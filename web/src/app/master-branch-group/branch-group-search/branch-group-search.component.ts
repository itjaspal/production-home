import { AuthenticationService } from './../../_service/authentication.service';
import { ActivatedRoute } from '@angular/router';
import { Dropdownlist } from './../../_model/dropdownlist';
import { DropdownlistService } from './../../_service/dropdownlist.service';
import { CommonSearchView } from './../../_model/common-search-view';
import { BranchGroupSearchView, BranchGroupView } from './../../_model/branchGroup';
import { Component, OnInit } from '@angular/core';
import { BranchGroupService } from '../../_service/branch-group.service';
import { forkJoin } from 'rxjs';
import { AppSetting } from '../../_constants/app-setting';
import { PageEvent } from '@angular/material';

@Component({
  selector: 'app-branch-group-search',
  templateUrl: './branch-group-search.component.html',
  styles: []
})
export class BranchGroupSearchComponent implements OnInit {

  constructor(
    private _branchGroupSvc: BranchGroupService,
    private _ddlSvc: DropdownlistService,
    private _actRoute:ActivatedRoute,
    private _authSvc: AuthenticationService
  ) { }

  public toppingList: any = [];
  public model: BranchGroupSearchView = new BranchGroupSearchView();
  public ddlStatus: any;
  public ddlBranch: Dropdownlist[] = [];
  public ddlDepartment: Dropdownlist[] = [];  
  public pageSizeOptions: number[] = AppSetting.pageSizeOptions;
  public pageEvent: any;
  actions: any = {};

  public data: CommonSearchView<BranchGroupView> = new CommonSearchView<BranchGroupView>();

  async ngOnInit() {    
    this.actions = this._authSvc.getActionAuthorization(this._actRoute);

    if (sessionStorage.getItem('session-branch-group-search') != null) {
      this.model = JSON.parse(sessionStorage.getItem('session-branch-group-search'));
      this.search();
    }

  }

  async ngOnDestroy() {
    this.saveSession();
  }

  async saveSession() {
    sessionStorage.setItem('session-branch-group-search', JSON.stringify(this.model));
  }

  async search(event: PageEvent = null) {

    if (event != null) {
      this.model.pageIndex = event.pageIndex;
      this.model.itemPerPage = event.pageSize;
    }

    this.data = await this._branchGroupSvc.search(this.model);
  }

  delete(row: BranchGroupView) {

  }

}

