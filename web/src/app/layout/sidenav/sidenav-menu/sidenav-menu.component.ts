import { AuthenticationService } from './../../../_service/authentication.service';
import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'my-app-sidenav-menu',
  styles: [],
  templateUrl: './sidenav-menu.component.html'
})

export class AppSidenavMenuComponent {

  public user: any;
  public allowDashboard: boolean = false;

  constructor(
    private authenticationService: AuthenticationService,
    private router: Router
  ) { }

  async ngOnInit() {
    this.user = this.authenticationService.getLoginUser();
    
    let userMenuGroup = this.user.menuGroups;
    console.log(userMenuGroup);
    //this.user.menuGroups = userMenuGroup.filter(x => x.menuFunctionGroupId != '01');

    // let dashMenu = userMenuGroup.filter(x => x.menuFunctionGroupId == '01');
    // this.allowDashboard = dashMenu.length > 0;
  }

  getRouterLink(menu) {

    let firstLinkChar = menu.menuURL.charAt(0);
    let isSlash = firstLinkChar == '/';

    if(!isSlash)
    {
      //console.log(this.user.username);
      window.open(menu.menuURL, '_self');
    }
    else
    {
      this.router.navigateByUrl(menu.menuURL);
    }
   
  }

}


