import { Component, OnInit } from '@angular/core';
import { APPCONFIG } from '../../config';
import { Router } from '@angular/router';
import { AuthenticationService } from '../../_service/authentication.service';

@Component({
  selector: 'my-app-mobile-header',
  styleUrls: ['./mobile-header.component.scss'],
  templateUrl: './mobile-header.component.html'
})

export class AppMobileHeaderComponent implements OnInit {
  public AppConfig: any;

  constructor(
    private authenticationService: AuthenticationService
  ) { }

  ngOnInit() {
    this.AppConfig = APPCONFIG;
  }

  logout() {
    this.authenticationService.logout();
  }
}
