import { MessageService } from './message.service';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { AuthenticationService } from './authentication.service';

@Injectable({
  providedIn: 'root'
})
export class AuthenGuard implements CanActivate {

  constructor(
    private route: Router,
    private _msgSvc: MessageService,
    private _authSvc: AuthenticationService
  ) { }

  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {

    if (sessionStorage.getItem('loginUser')) {
      // logged in so return true

      let user = this._authSvc.getLoginUser();

      if(user.isDefaultPassword){
        this._msgSvc.warningPopup("กรุณาเปลี่ยนรหัสผ่านก่อนใช้งาน")
        this.route.navigate(['/app/user/change-password']);
      }

      return true;
    }

    //this._msgSvc.errorPopup("กรุณา LOGIN เพื่อเข้าใช้งานระบบ");
    //not logged in so redirect to login page with the return url
    this.route.navigate(['/login']);
    //this.route.navigate(['/stock']);
    return false;
  }
}
