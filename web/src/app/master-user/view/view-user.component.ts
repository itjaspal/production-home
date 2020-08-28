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
  selector: 'app-view-user',
  templateUrl: './view-user.component.html',
  styleUrls: ['./view-user.component.scss']
})
export class ViewUserComponent extends BaseComponent  implements OnInit {

  public data: any = {};

  formGroup: FormGroup;

  userEntityFormGroup: FormGroup;

  get f() { return this.formGroup.controls; }
  get ue() { return this.userEntityFormGroup.controls; }

  constructor(
    private _userService: UserService,
    public messageService: MessageService,
    private route: ActivatedRoute,
  ) {
    super();
  }

  async ngOnInit() {

    this.data = await this._userService.get(this.route.snapshot.params.id);
    console.log(this.data);

  }


}
