import { Dropdownlist } from './../../_model/dropdownlist';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthenticationService } from './../../_service/authentication.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { DropdownlistService } from '../../_service/dropdownlist.service';
import { UserService } from '../../_service/user.service';
import { MessageService } from '../../_service/message.service';
import { AppSetting } from '../../_constants/app-setting';
import { OrganizationService } from '../../_service/organization.service';
import { MatOption } from '@angular/material';

@Component({
  selector: 'app-inq-user',
  templateUrl: './inq-user.component.html',
  styles: []
})
export class InqUserComponent implements OnInit {
  
  public model: any = {
    isPC: false,
    pageIndex: 1,
    itemPerPage: AppSetting.itemPerPage,
    username: "",
    name: "",
    branchId: undefined,
    branchGroupId: undefined,
    branchList: []
  };
  public data: any = {};
  user: any = {};
  public userRoleLists: any;
  public pageSizeOptions: number[] = AppSetting.pageSizeOptions;
  public branchGroupLists: any;

  ddlBranchGroups: Dropdownlist[] = [];
  ddlBranches: Dropdownlist[] = [];

  fbGroup: FormGroup;

  @ViewChild("allSelected") private allSelected: MatOption;

  constructor(
    private _ddlSvc: DropdownlistService,
    private _userService: UserService,
    private messageService: MessageService,
    private _branchService: OrganizationService,
    private _authSvc: AuthenticationService,
    private fb: FormBuilder
  ) { }

  async ngOnInit() {
    this.buildForm();

    this.user = await this._authSvc.getLoginUser();

    this.userRoleLists = await this._ddlSvc.getDdlUserRoles(this.model.isPC, this.user.username);
    

    //#region filter branchList

    //#region select one
    if (this.user.username == "admin") {
      this.ddlBranchGroups = await this._ddlSvc.getDdlBranchGroup();
    } else {
      this.ddlBranchGroups = this._authSvc.getUserBranchGroupes();
    }
    if(this.ddlBranchGroups.length == 1){
      this.model.branchGroupId = this.ddlBranchGroups[0].key;
      this.getBranchInGroup(this.model.branchGroupId);
    }
    //#endregion

    //#region select multiple
    // if (this.user.username == "admin") {
    //   this.branchGroupLists = await this._branchService.inquiryBranchs();
    // } else {
    //   this.branchGroupLists = [];
    //   var bGroups = this._authSvc.getUserBranchGroupes();

    //   bGroups.forEach(bGroup => {

    //     var allowBGroup = { branchList: [], branchGroupId: bGroup.key, branchGroupCode: "", branchGroupName: bGroup.value };
    //     var allowBranchs = this._authSvc.getUserBranches(bGroup.key);

    //     allowBranchs.forEach(allowBranch => {
    //       allowBGroup.branchList.push({ branchId: allowBranch.key, branchGroupId: bGroup.key, branchCode: "", branchNameThai: allowBranch.value });
    //     });

    //     this.branchGroupLists.push(allowBGroup);
    //   });

    // }
    //#endregion

    //#endregion
  }

  async getBranchInGroup(branchGroupId: number) {
    this.model.branchId = undefined;

    if (this.user.username == "admin") {
      this.ddlBranches = await this._ddlSvc.getDdlBranchInGroup(branchGroupId);
    } else {

      this.ddlBranches = this._authSvc.getUserBranches(branchGroupId);

    }
    if(this.ddlBranches.length == 1){
      this.model.branchId = this.ddlBranches[0].key;
    }
  }

  buildForm() {
    this.fbGroup = this.fb.group({
      isPC: [null, []],
      branchList: [null, []],
      username: [null, []],
      name: [null, []],
      roleId: [null, []],
      status: [null, []],
      branchId: [null, [Validators.required]],
      branchGroupId: [null, [Validators.required]],
    })
  }

  tosslePerOne(all) {
    if (this.allSelected.selected) {
      this.allSelected.deselect();
      return false;
    }

    this.allSelected.select();
    if (this.branchGroupLists.length == 0) {
      this.allSelected.deselect();
    } else {

      let count: number = 0;
      this.branchGroupLists.forEach(group => {
        count += group.branchList.length;
      });
      if (this.fbGroup.controls.userType.value.length == count)
        this.allSelected.select();
    }

  }

  toggleAllSelection() {

    if (this.allSelected.selected) {

      let branchListId: number[] = [];
      this.branchGroupLists.forEach(group => {
        group.branchList.forEach(brn => {
          branchListId.push(brn.branchId);
        });
      });

      this.fbGroup.controls.branchList
        .patchValue([...branchListId.map(branchId => branchId), 0]);
    } else {
      this.fbGroup.controls.branchList.patchValue([]);
    }
  }

  async search() {

    this.model.createUser = this.user.username;
    this.data = await this._userService.searchUsers(this.model);
    console.log(this.data);
  }

  async changePCUser(isPc) {
    this.userRoleLists = await this._ddlSvc.getDdlUserRoles(isPc,this.user.username);
    this.model.pageIndex = 1;
    await this.search();
  }

  async delete(user) {
    let res: any = await this._userService.delete(user);

    this.messageService.successPopup(res.message);

    await this.search();
  }

}
