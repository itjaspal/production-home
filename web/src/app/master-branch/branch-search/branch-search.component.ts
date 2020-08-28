import { AuthenticationService } from './../../_service/authentication.service';
import { ActivatedRoute } from '@angular/router';
import { Dropdownlist } from './../../_model/dropdownlist';
import { DropdownlistService } from './../../_service/dropdownlist.service';
import { CommonSearchView } from './../../_model/common-search-view';
import { BranchSearchView, BranchView } from './../../_model/branch';
import { Component, OnInit } from '@angular/core';
import { OrganizationService } from '../../_service/organization.service';
import { forkJoin } from 'rxjs';
import { AppSetting } from '../../_constants/app-setting';
import { PageEvent } from '@angular/material';

@Component({
  selector: 'app-branch-search',
  templateUrl: './branch-search.component.html',
  styles: []
})
export class BranchSearchComponent implements OnInit {

  constructor(
    private _orgSvc: OrganizationService,
    private _ddlSvc: DropdownlistService,
    private _actRoute: ActivatedRoute,
    private _authSvc: AuthenticationService
  ) { }

  public toppingList: any = [];
  public model: BranchSearchView = new BranchSearchView();
  public ddlStatus: Dropdownlist[] = [];
  public ddlBranchGroup: Dropdownlist[] = [];
  public pageSizeOptions: number[] = AppSetting.pageSizeOptions;
  public pageEvent: any;

  // public data: CommonSearchView<BranchView> = new CommonSearchView<BranchView>();
  public data: any = {};
  actions: any = {};

  async ngOnInit() {

    this.actions = this._authSvc.getActionAuthorization(this._actRoute);

    forkJoin([
      this._ddlSvc.getDdlBranchGroup(),
      this._ddlSvc.getDdlBranchStatus()
    ]).subscribe(result => {
      this.ddlBranchGroup = result[0];
      this.ddlStatus = result[1];
    });

    if (sessionStorage.getItem('session-branch-search') != null) {
      this.model = JSON.parse(sessionStorage.getItem('session-branch-search'));
      if (this.model.branchGroupId != undefined) {
        this.search();
      }

    }

  }

  async ngOnDestroy() {
    this.saveSession();
  }

  async saveSession() {
    sessionStorage.setItem('session-branch-search', JSON.stringify(this.model));
  }

  async search(event: PageEvent = null) {

    if (event != null) {
      this.model.pageIndex = event.pageIndex;
      this.model.itemPerPage = event.pageSize;
    }

    this.data = await this._orgSvc.searchBranch(this.model);
  }

}

