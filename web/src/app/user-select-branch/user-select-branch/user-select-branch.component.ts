import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from '../../_service/authentication.service';

@Component({
  selector: 'app-user-select-branch',
  templateUrl: './user-select-branch.component.html',
  styleUrls: ['./user-select-branch.component.scss']
})
export class UserSelectBranchComponent implements OnInit {
  
  public user;

  constructor(
    private authenticationService: AuthenticationService,
    private router: Router,
  ) { }

  ngOnInit() {

    this.user = this.authenticationService.getLoginUser();
    console.log(this.user);

    if (this.user.userEntityPrvlgList.length == 1) {
      this.select(this.user.userEntityPrvlgList[0]);
    } else if (this.user.userEntityPrvlgList.length == 0) {
      this.router.navigateByUrl('/app/home');
    }
  }

  async select(branch) {

    let res: any = await this.authenticationService.getPcUserRole(branch);

    // this.router.navigateByUrl('/app/mobile-navigator');
    this.router.navigateByUrl('/app/home');

  }

  logout() {
    this.authenticationService.logout();
  }

}
