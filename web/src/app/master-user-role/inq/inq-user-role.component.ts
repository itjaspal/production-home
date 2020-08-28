import { Component, OnInit } from '@angular/core';
import { AppSetting } from '../../_constants/app-setting';
import { UserRoleService } from '../../_service/user-role.service';
import { MessageService } from '../../_service/message.service';
import { AuthenticationService } from '../../_service/authentication.service';

@Component({
  selector: 'app-inq-user-role',
  templateUrl: './inq-user-role.component.html',
  styles: []
})
export class InqUserRoleComponent implements OnInit {

  public user: any;

  public model: any = {
    isPC: false,
    pageIndex: 1,
    itemPerPage: AppSetting.itemPerPage,
    name: "",
    userName: ""
  };

  public data: any = {};
  public pageSizeOptions: number[] = AppSetting.pageSizeOptions;

  constructor(
    private _userRoleService: UserRoleService,
    private messageService: MessageService,
    private _authSvc: AuthenticationService,
  ) { }

  async ngOnInit() {
    this.user = await this._authSvc.getLoginUser();
  }

  async search() {
    this.model.createUser = this.user.username;
    this.data = await this._userRoleService.search(this.model);
    console.log(this.data);
  }

  async delete(data) {    
    let res: any = await this._userRoleService.delete(data);

    this.messageService.successPopup(res.message);

    await this.search();
  }

}
