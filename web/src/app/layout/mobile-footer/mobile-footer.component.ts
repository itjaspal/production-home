import { Component, OnInit } from '@angular/core';
import { APPCONFIG } from '../../config';
import { AuthenticationService } from '../../_service/authentication.service';
import { environment } from '../../../environments/environment';
import { UserFunctionConstant } from '../../_constants/user-functions.constant';

@Component({
  selector: 'my-app-mobile-footer',
  styleUrls: ['./mobile-footer.component.scss'],
  templateUrl: './mobile-footer.component.html'
})

export class AppMobileFooterComponent implements OnInit {
  public AppConfig: any;
  public data: any;
  public version;

  constructor(
    private authenticationService: AuthenticationService,
  ) { }

  ngOnInit() {
    this.AppConfig = APPCONFIG;
    this.version = environment.version;
    this.data = this.authenticationService.getLoginUser();
  }
}
