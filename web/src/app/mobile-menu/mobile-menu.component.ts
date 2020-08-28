import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../_service/authentication.service';
import { Router } from '@angular/router';
import { UserFunctionConstant } from '../_constants/user-functions.constant';

@Component({
  selector: 'mobile-menu-app',
  templateUrl: './mobile-menu.component.html',
  styleUrls: ['./mobile-menu.component.scss']
})
export class MobileMenuComponent {

  public user:any;

  constructor(
    private authenticationService: AuthenticationService,
    private router: Router
  ) { }

  async ngOnInit() {
    this.user = this.authenticationService.getLoginUser();
  }

  selectMenu(func){
    console.log(func);
    
    this.router.navigateByUrl(func.menuURL);
    
  }

}
