import { Component, OnInit } from '@angular/core';
import { DropdownlistService } from '../../_service/dropdownlist.service';
import { UserService } from '../../_service/user.service';
import { AppSetting } from '../../_constants/app-setting';
import { OrganizationService } from '../../_service/organization.service';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { BaseComponent } from '../../_common/base.component';
import { MessageService } from '../../_service/message.service';
import { Router } from '@angular/router';
import { UserRoleService } from '../../_service/user-role.service';
import { AuthenticationService } from '../../_service/authentication.service';

@Component({
  selector: 'app-create-user-role',
  templateUrl: './create-user-role.component.html',
  styleUrls: ['./create-user-role.component.scss']
})
export class CreateUserRoleComponent extends BaseComponent implements OnInit {

  public user: any;

  public functionGroup: any = [];

  public data: any = {};

  public functionRequired = false;

  formGroup: FormGroup;

  userEntityFormGroup: FormGroup;

  get f() { return this.formGroup.controls; }

  constructor(
    private fb: FormBuilder,
    public messageService: MessageService,
    private router: Router,
    private userRoleService: UserRoleService,
    private _userService: UserService,
    private _authSvc: AuthenticationService,
  ) {
    super();
  }

  async ngOnInit() {

    this.formGroup = this.fb.group({});

    this.formGroup.addControl('name', new FormControl("", [Validators.required]));
    this.formGroup.addControl('isPC', new FormControl("", null));

    this.data.isPC = false;

    this.data.userEntity = [];

    this.functionGroup = await this.userRoleService.getFunctionGroup(this.data.isPC);
    
    console.log(this.functionGroup);

    this.user = await this._authSvc.getLoginUser();

  }

  async changePCUser(isPc) {

    this.functionGroup = await this.userRoleService.getFunctionGroup(isPc);
    console.log(this.functionGroup);

  }


  checkAllGroup(isAll, group) {
    group.menuFunctionList.forEach(func => {
      func.menuFunctionActionList.forEach(action => {
        action.isSelected = isAll;
      });
      
    });
  }

  async create() {

    this.functionRequired = false;

    console.log(this.formGroup);

    if (this.formGroup.invalid) {

      await this.markFormGroupTouched(this.formGroup);
      this.messageService.errorPopup('กรุณากรอกข้อมูลให้สมบูรณ์');
      return;
    }

    this.data.functions = [];
    this.functionGroup.forEach(group => {
      group.menuFunctionList.forEach(func => {
        func.menuFunctionActionList.forEach(action => {
          if (action.isSelected) {

            let functionTemp = {
              groupId: func.menuFunctionGroupId,
              functionId: func.menuFunctionId,
              actionId: action.menuFunctionActionId
            }
  
            this.data.functions.push(functionTemp);
          }
        });
        
      });
    });

    console.log(this.data);

    if (this.data.functions.length == 0) {
      this.messageService.errorPopup('กรุณาเลือกสิทธิ์การเข้าถึง');
      this.functionRequired = true;
      return;
    }

    console.log(this.user);

    this.data.createUser = this.user.username;

    console.log(this.data);

    let res: any = await this.userRoleService.create(this.data);
    console.log(res);

    await this.messageService.successPopup(res.message);

    this.router.navigateByUrl('/app/user-role');

  }

}
