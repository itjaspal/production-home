import { AuthenticationService } from './../../_service/authentication.service';
import { Component, OnInit } from '@angular/core';
import { APPCONFIG } from '../../config';

@Component({
  selector: 'my-app-header',
  styleUrls: ['./header.component.scss'],
  templateUrl: './header.component.html'
})

export class AppHeaderComponent implements OnInit {

  public AppConfig: any;
  public user: any;

  constructor(
    private authenticationService: AuthenticationService,
  ) { }

  async ngOnInit() {
    this.AppConfig = APPCONFIG;
    this.user = this.authenticationService.getLoginUser();
  }

  logout() {
    this.authenticationService.logout();
  }
}
