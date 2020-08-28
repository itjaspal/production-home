import { Component, OnInit } from '@angular/core';
import { DropdownlistService } from '../../_service/dropdownlist.service';
import { UserService } from '../../_service/user.service';
import { AppSetting } from '../../_constants/app-setting';
import { OrganizationService } from '../../_service/organization.service';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { BaseComponent } from '../../_common/base.component';
import { MessageService } from '../../_service/message.service';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-reset-user',
  templateUrl: './reset-user.component.html',
  styleUrls: ['./reset-user.component.scss']
})
export class ResetPasswordUserComponent extends BaseComponent  implements OnInit {

  public data: any = {};

  public user: any;

  constructor(
    private _userService: UserService,
    public messageService: MessageService,
    private route: ActivatedRoute,
    private router: Router,
  ) {
    super();
  }

  async ngOnInit() {

    this.data = await this._userService.get(this.route.snapshot.params.id);
    console.log(this.data);

  }

  async resetPassword(){
    let res: any = await this._userService.resetPassword(this.data.username);
    await this.messageService.successPopup(res.message);

    this.router.navigateByUrl('/app/user');
  }


}
