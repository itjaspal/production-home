import { MessageService } from './message.service';
import { HttpClient } from '@angular/common/http';
import { environment } from './../../environments/environment';
import { Injectable } from '@angular/core';
import { UserRoleService } from './user-role.service';
import { Router, ActivatedRoute } from '@angular/router';
import * as CryptoJS from 'crypto-js';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  constructor(
    private http: HttpClient,
    private userRoleService: UserRoleService,
    private router: Router,
    private _msgSvc: MessageService,
  ) { }

  public async login(username, password) {
    let param = {
      username: username,
      //password: CryptoJS.SHA3(password).toString()
      password: password
    };

    let user: any = await this.http.post(environment.API_URL + 'authen/login', param).toPromise()

    sessionStorage.clear();
    sessionStorage.setItem('loginUser', JSON.stringify(user));

    return user;
  }


  public async getPcUserRole(branch) {

    let param = {
      userRoleId: branch.userRoleId
    }

    let menuGroups: any = await this.http.post(environment.API_URL + 'authen/pc/getUserRole', param).toPromise();

    let user = this.getLoginUser();
    user.branch = branch;
    user.menuGroups = menuGroups;
    sessionStorage.setItem('loginUser', JSON.stringify(user));

    return user;

  }

  public getLoginUser() {
    return JSON.parse(sessionStorage.getItem('loginUser'));
  }

  public logout() {

    this._msgSvc.confirmPopup("ยืนยันออกจากระบบ", result => {
      if (result) {
        sessionStorage.clear();
        this.router.navigateByUrl('/login');
      }
    })

  }

  public getActionAuthorization(_activateRoute: ActivatedRoute): any {
    try {
      const ROUTE_DATA_FUNCTION_URL: string = "parentUrl";

      //get menuFunctionId from routing
      let parentUrl = _activateRoute.snapshot.data[ROUTE_DATA_FUNCTION_URL];

      //get menuFunction from user login
      let user = this.getLoginUser();

      let actions: any = {};
      for (var i = 0; i < user.menuGroups.length; i++) {
        let functions = user.menuGroups[i].menuFunctionList.filter(x => x.menuURL == parentUrl);

        if (functions.length > 0) {
          actions = functions[0].actions;
          break;
        }
      }

      return actions;
    } catch (e) {
      return {};
    }
  }

  public getUserBranchGroupes(): any[] {
    try {

      //get menuFunction from user login
      let user = this.getLoginUser();

      return user.userBranchGroupes;

    } catch (e) {
      return [];
    }
  }

  public getUserBranches(branchGroupId=0): any[] {
    try {

      //get menuFunction from user login
      let user = this.getLoginUser();

      if (branchGroupId == 0) return user.userBranches;

      return user.userBranches.filter(x => x.parentKey == branchGroupId);

    } catch (e) {
      return [];
    }
  }

}
