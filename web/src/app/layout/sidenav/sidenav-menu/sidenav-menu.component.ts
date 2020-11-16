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

    let menu_id = menu.menuFunctionId;

    if(menu_id == "MOBB030002" && this.user.branch.entity_code == "HMSTK")
    {
      let menuURL = "/app/job-stk";
      this.router.navigateByUrl(menuURL);
    }
    
    
    else if(menu_id == "MOBB030004" && this.user.branch.entity_code == "HMSTK")
    {
      let menuURL = "/app/scaninproc-stk";
      this.router.navigateByUrl(menuURL);
    }
    else
    {
      this.router.navigateByUrl(menu.menuURL);
    }

   
  }

}


