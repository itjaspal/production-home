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
  selector: 'app-branch-search-assignProduct',
  templateUrl: './branch-search-assignProduct.component.html',
  styles: []
})
export class BranchSearchAssignProductComponent implements OnInit {

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
  public ddlBranch: Dropdownlist[] = [];
  public pageSizeOptions: number[] = AppSetting.pageSizeOptions;
  public pageEvent: any;

  // public data: CommonSearchView<BranchView> = new CommonSearchView<BranchView>();
  public data: any = {};
  actions: any = {};

  async ngOnInit() {

    this.actions = this._authSvc.getActionAuthorization(this._actRoute);

    this.ddlBranchGroup = this._authSvc.getUserBranchGroupes();

    console.log( this.ddlBranchGroup);

    forkJoin([  
      this._ddlSvc.getDdlBranchStatus()
    ]).subscribe(result => {
      this.ddlStatus = result[0];
    });

    if (sessionStorage.getItem('session-branch-search') != null) {
      this.model = JSON.parse(sessionStorage.getItem('session-branch-search'));
      if (this.model.branchGroupId != undefined) {
        this.ddlBranch = await this._authSvc.getUserBranches(this.model.branchGroupId);      
        if (this.model.branchId != undefined) {
          this.search();
        }
      }

    }

  }

  async getBranchInGroup()
  {
    this.ddlBranch = await this._authSvc.getUserBranches(this.model.branchGroupId);   
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

    console.log(this.model);

    this.data = await this._orgSvc.searchBranchById(this.model);
  }

}

