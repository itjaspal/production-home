import { ActivatedRoute } from '@angular/router';
import { AuthenticationService } from './../../_service/authentication.service';
import { Dropdownlist } from './../../_model/dropdownlist';
import { DropdownlistService } from './../../_service/dropdownlist.service';
import { CommonSearchView } from './../../_model/common-search-view';
import { DepartmentSearchView, DepartmentView } from './../../_model/department';
import { Component, OnInit } from '@angular/core';
import { DepartmentService } from '../../_service/department.service';
import { forkJoin } from 'rxjs';
import { AppSetting } from '../../_constants/app-setting';
import { PageEvent } from '@angular/material';

@Component({
  selector: 'app-department-search',
  templateUrl: './department-search.component.html',
  styles: []
})
export class DepartmentSearchComponent implements OnInit {

  constructor(
    private _departmentSvc: DepartmentService,
    private _ddlSvc: DropdownlistService,
    private _authSvc: AuthenticationService,
    private _actRoute: ActivatedRoute
  ) { }

  public toppingList: any = [];
  public model: DepartmentSearchView = new DepartmentSearchView();
  public ddlStatus: any;
  public ddlBranch: Dropdownlist[] = [];
  public ddlDepartment: Dropdownlist[] = [];
  public pageSizeOptions: number[] = AppSetting.pageSizeOptions;
  public pageEvent: any;

  public data: CommonSearchView<DepartmentView> = new CommonSearchView<DepartmentView>();
  actions: any = {};

  async ngOnInit() {
    this.actions = this._authSvc.getActionAuthorization(this._actRoute);

    this.ddlStatus = await this._ddlSvc.getDdlBranchStatus();

    if (sessionStorage.getItem('session-department-search') != null) {
      this.model = JSON.parse(sessionStorage.getItem('session-department-search'));
      this.search();
    }
  }

  async ngOnDestroy() {
    this.saveSession();
  }

  async saveSession() {
    sessionStorage.setItem('session-department-search', JSON.stringify(this.model));
  }

  async search(event: PageEvent = null) {

    if (event != null) {
      this.model.pageIndex = event.pageIndex;
      this.model.itemPerPage = event.pageSize;
    }

    this.data = await this._departmentSvc.search(this.model);
  }

  delete(row: DepartmentView) {

  }

}

