import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../_service/authentication.service';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { BaseComponent } from '../_common/base.component';
import { MessageService } from '../_service/message.service';
import { environment } from '../../environments/environment';

@Component({
  selector: 'app-user-login',
  templateUrl: './user-login.component.html'
})
export class UserLoginComponent extends BaseComponent implements OnInit {

  formGroup: FormGroup;

  public data: any = {
    username: '',
    password: ''
  };
  version: string = "";

  get f() { return this.formGroup.controls; }

  constructor(
    private authenticationService: AuthenticationService,
    private router: Router,
    private fb: FormBuilder,
    private messageService: MessageService
  ) {
    super();
  }

  
  async ngOnInit() {

    localStorage.clear();
    this.version = environment.version;

    this.formGroup = this.fb.group({});

    this.formGroup.addControl('password', new FormControl("", [Validators.required]));
    this.formGroup.addControl('username', new FormControl("", [Validators.required]));
  }

  async login() {
    if (this.formGroup.invalid) {

      await this.markFormGroupTouched(this.formGroup);
      return;
    }

    let user: any = await this.authenticationService.login(this.data.username, this.data.password);

    // if (user.isPC) {
    //   this.router.navigateByUrl('/select-branch');
    // } else {
    //   if (user.menuGroups.length > 0 && user.menuGroups[0].menuFunctionGroupId == '01') {
    //     this.router.navigateByUrl('/app/dashboard');
    //   } else {
        this.router.navigateByUrl('/app/home');
    //  }
    //}
  }

}
