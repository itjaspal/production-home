import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../_service/authentication.service';

@Component({
  selector: 'mobile-profile-app',
  templateUrl: './mobile-profile.component.html',
  styleUrls: ['./mobile-profile.component.scss']
})
export class MobileProfileComponent {

  public user;

  constructor(
    private authenticationService: AuthenticationService,
  ) { }

  async ngOnInit() {
    this.user = this.authenticationService.getLoginUser();
  }

}
