import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../../_service/authentication.service';
import { ActivatedRoute } from '@angular/router';
import { BaseComponent } from '../../_common/base.component';
import { MessageService } from '../../_service/message.service';
import { DropdownlistService } from '../../_service/dropdownlist.service';
import { PageEvent } from '@angular/material';
import { Dropdownlist } from '../../_model/dropdownlist';
import { AppSetting } from '../../_constants/app-setting';
import { MenuGroupView, MenuGroupSearchView } from '../../_model/menugroup';
import { CommonSearchView } from '../../_model/common-search-view';
import { MenuGroupService } from '../../_service/menu-group.service';



@Component({
  selector: 'app-menu-group-search',
  templateUrl: './menu-group-search.component.html',
  styleUrls: ['./menu-group-search.component.scss']
})
export class MenuGroupSearchComponent implements OnInit {
  constructor(
    private _MenuGroupSvc: MenuGroupService,
    private _ddlSvc: DropdownlistService,
    private _actRoute:ActivatedRoute,
    private _authSvc: AuthenticationService
  ) { }

  public toppingList: any = [];
  public model: MenuGroupSearchView = new MenuGroupSearchView();
  public ddlStatus: any;
  public ddlBranch: Dropdownlist[] = [];
  public ddlDepartment: Dropdownlist[] = [];  
  public pageSizeOptions: number[] = AppSetting.pageSizeOptions;
  public pageEvent: any;
  actions: any = {};

  public data: CommonSearchView<MenuGroupView> = new CommonSearchView<MenuGroupView>();

  async ngOnInit() {    
    this.actions = this._authSvc.getActionAuthorization(this._actRoute);

    if (sessionStorage.getItem('session-menu-group-search') != null) {
      this.model = JSON.parse(sessionStorage.getItem('session-menu-group-search'));
      this.search();
    }

    

  }

  async ngOnDestroy() {
    this.saveSession();
  }

  async saveSession() {
    sessionStorage.setItem('session-menu-group-search', JSON.stringify(this.model));
  }

  async search(event: PageEvent = null) {

    if (event != null) {
      this.model.pageIndex = event.pageIndex;
      this.model.itemPerPage = event.pageSize;
    }

    this.data = await this._MenuGroupSvc.search(this.model);
  }

  delete(row: MenuGroupView) {

  }
}
