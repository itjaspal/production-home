import { Component, OnInit } from '@angular/core';
import { MenuService } from '../../_service/menu.service';
import { DropdownlistService } from '../../_service/dropdownlist.service';
import { ActivatedRoute } from '@angular/router';
import { AuthenticationService } from '../../_service/authentication.service';
import { MenuSearchView, MenuView } from '../../_model/menu';
import { Dropdownlist } from '../../_model/dropdownlist';
import { AppSetting } from '../../_constants/app-setting';
import { CommonSearchView } from '../../_model/common-search-view';
import { PageEvent } from '@angular/material';

@Component({
  selector: 'app-menu-search',
  templateUrl: './menu-search.component.html',
  styleUrls: ['./menu-search.component.scss']
})
export class MenuSearchComponent implements OnInit {

  constructor(
    private _MenuSvc: MenuService,
    private _ddlSvc: DropdownlistService,
    private _actRoute:ActivatedRoute,
    private _authSvc: AuthenticationService
  ) { }

  public toppingList: any = [];
  public model: MenuSearchView = new MenuSearchView();
  public ddlStatus: any;
  public ddlBranch: Dropdownlist[] = [];
  public ddlDepartment: Dropdownlist[] = [];  
  public pageSizeOptions: number[] = AppSetting.pageSizeOptions;
  public pageEvent: any;
  actions: any = {};

  public data: CommonSearchView<MenuView> = new CommonSearchView<MenuView>();

  async ngOnInit() {    
    this.actions = this._authSvc.getActionAuthorization(this._actRoute);

    if (sessionStorage.getItem('session-menu-search') != null) {
      this.model = JSON.parse(sessionStorage.getItem('session-menu-search'));
      this.search();
    }

    

  }

  async ngOnDestroy() {
    this.saveSession();
  }

  async saveSession() {
    sessionStorage.setItem('session-menu-search', JSON.stringify(this.model));
  }

  async search(event: PageEvent = null) {

    if (event != null) {
      this.model.pageIndex = event.pageIndex;
      this.model.itemPerPage = event.pageSize;
    }

    this.data = await this._MenuSvc.search(this.model);
  }

  delete(row: MenuSearchView) {

  }

}
