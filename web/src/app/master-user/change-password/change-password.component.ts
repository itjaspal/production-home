import { Component, OnInit } from '@angular/core';
import { UserService } from '../../_service/user.service';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { BaseComponent } from '../../_common/base.component';
import { MessageService } from '../../_service/message.service';
import { Router } from '@angular/router';
import { AuthenticationService } from '../../_service/authentication.service';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss']
})
export class ChangePasswordComponent extends BaseComponent implements OnInit {

  public data: any = {};
  public user: any = {};

  formGroup: FormGroup;

  get f() { return this.formGroup.controls; }

  constructor(
    private _userService: UserService,
    private authenticationService: AuthenticationService,
    private fb: FormBuilder,
    private router: Router,
    public messageService: MessageService) {
    super();
  }

  async ngOnInit() {

    this.user = this.authenticationService.getLoginUser();

    this.formGroup = this.fb.group({});

    this.formGroup.addControl('oldPassword', new FormControl("", [
      // Validators.minLength(8),
      Validators.required
    ]));

    this.formGroup.addControl('newPassword', new FormControl("", [
      Validators.minLength(4),
      Validators.maxLength(8),
      Validators.required
    ]));

    this.formGroup.addControl('confirmPassword', new FormControl("", [
      Validators.minLength(4),
      Validators.maxLength(8),
      Validators.required
    ]));

    this.formGroup.setValidators(this.checkPasswords);
  }

  async changePassword() {
    
    if (this.formGroup.invalid) {
      await this.markFormGroupTouched(this.formGroup);
      this.messageService.errorPopup('กรุณากรอกข้อมูลให้สมบูรณ์');
      return;
    }

    let res: any = await this._userService.changePassword(this.user.username, this.data.oldPassword, this.data.newPassword);

    await this.messageService.successPopup(res.message);

    if (this.user.isDefaultPassword) {
      this.router.navigateByUrl('/login');
    } else {
      if (this.user.menuGroups[0].menuFunctionGroupId == '01') {
        //this.router.navigateByUrl('/app/dashboard');
        this.router.navigateByUrl('/app/mobile-navigator');
      } else {
        this.router.navigateByUrl('/app/mobile-navigator');
      }
    }

  }


  checkPasswords(group: FormGroup) {
    let pass = group.controls.newPassword.value;
    let confirmPass = group.controls.confirmPassword.value;

    return pass === confirmPass ? null : { notSame: true }
  }

}
