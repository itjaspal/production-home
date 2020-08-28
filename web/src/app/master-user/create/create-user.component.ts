import { Component, OnInit } from '@angular/core';
import { DropdownlistService } from '../../_service/dropdownlist.service';
import { UserService } from '../../_service/user.service';
import { AppSetting } from '../../_constants/app-setting';
import { OrganizationService } from '../../_service/organization.service';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { BaseComponent } from '../../_common/base.component';
import { MessageService } from '../../_service/message.service';
import { Router } from '@angular/router';
import { AuthenticationService } from '../../_service/authentication.service';

@Component({
  selector: 'app-create-user',
  templateUrl: './create-user.component.html',
  styleUrls: ['./create-user.component.scss']
})
export class CreateUserComponent extends BaseComponent  implements OnInit {

  public data: any = {};
  public departmentLists: any;
  public userRoleLists: any;
  public branchGroupLists: any;

  public user: any;

  public selectBranch;
  public selectUserRole;
  selectEntities = new Map<string, boolean>();  

  public userEntityRequired = false;

  formGroup: FormGroup;

  userEntityFormGroup: FormGroup;

  get f() { return this.formGroup.controls; }
  get ue() { return this.userEntityFormGroup.controls; }

  constructor(
    private _authSvc: AuthenticationService,
    private _ddlSvc: DropdownlistService,
    private _userService: UserService,
    private _branchService: OrganizationService,
    private fb: FormBuilder,
    public messageService: MessageService,
    private router: Router
  ) {
    super();
  }

  async ngOnInit() {

    this.formGroup = this.fb.group({});

    this.formGroup.addControl('name', new FormControl("", [Validators.required]));
    this.formGroup.addControl('username', new FormControl("", [Validators.required]));
    this.formGroup.addControl('userrole', new FormControl("", [Validators.required]));
    this.formGroup.addControl('department', new FormControl("", [Validators.required]));
    this.formGroup.addControl('isPC', new FormControl("", null));

    
    // this.userEntityFormGroup = this.formGroup.controls['userEntity'] as FormGroup;


    this.user = this._authSvc.getLoginUser();

    this.userEntityFormGroup = this.fb.group({});

    this.userEntityFormGroup.addControl('selectUserRole', new FormControl("", [Validators.required]));
    this.userEntityFormGroup.addControl('selectBranch', new FormControl("", [Validators.required]));

    this.data.isPC = false;

    this.userRoleLists = await this._ddlSvc.getDdlUserRoles(this.data.isPC, this.user.username);

    //this.branchGroupLists = await this._branchService.inquiryBranchs();

    if (this.user.username == "admin")
    {
      this.branchGroupLists = await this._branchService.inquiryBranchs();      
    }else{
      this.branchGroupLists=[];
      var bGroups = this._authSvc.getUserBranchGroupes();

      console.log(bGroups);

      bGroups.forEach(bGroup=>{
        console.log(bGroup);
        var allowBGroup = {branchList: [], branchGroupId: bGroup.key, branchGroupCode: "", branchGroupName: bGroup.value};
        var allowBranchs = this._authSvc.getUserBranches(bGroup.key);

        console.log(allowBranchs);

        allowBranchs.forEach(allowBranch=>{
          allowBGroup.branchList.push({branchId: allowBranch.key, branchGroupId: bGroup.key, branchCode: "", branchNameThai: allowBranch.value});
        });

        this.branchGroupLists.push(allowBGroup);
      });

    }

    console.log(this.branchGroupLists);
    
    this.departmentLists = await this._ddlSvc.getDdlDepartments();

    this.data.userEntity = [];

  }



  async changePCUser(isPc){
    
    if (isPc == false) {
      this.formGroup.addControl('userrole', new FormControl("", Validators.required));
      this.formGroup.addControl('department', new FormControl("", Validators.required));
    } else {
      this.formGroup.removeControl('userrole');
      this.formGroup.removeControl('department');
    }

    this.data.userEntity = [];
    this.selectEntities = new Map<string, boolean>();

    this.userRoleLists = await this._ddlSvc.getDdlUserRoles(isPc, this.user.username);

  }


  checkAllGroup(isAll,group){
    group.branchList.forEach(branch => {
      branch.isSelected = isAll;
    });
  }

  async addEntity(){
    console.log(this.userEntityFormGroup);

    if (this.userEntityFormGroup.invalid) {
      await this.markFormGroupTouched(this.userEntityFormGroup);
      // this.messageService.errorPopup('กรุณากรอกข้อมูลให้สมบูรณ์');
      return;
    }

    if( this.selectEntities.get(this.selectUserRole.key+this.selectBranch.branchNameThai) ){
      return;
    }

    this.selectEntities.set(this.selectUserRole.key+this.selectBranch.branchNameThai,true);

    let userEntity = {
      userRole: this.selectUserRole,
      branch: this.selectBranch
    }    

    this.selectUserRole = undefined;
    this.selectBranch = undefined;

    this.data.userEntity.push(userEntity);

    await this.markFormGroupUnTouched(this.userEntityFormGroup);

    console.log(this.data.userEntity);

  }

  removeUserEntity(userEntity){
    let index = this.data.userEntity.indexOf(userEntity);
    this.data.userEntity.splice(index, 1);
    this.selectEntities.delete(userEntity.userRole.key+userEntity.branch.branchNameThai);
  }

  async create(){

    this.userEntityRequired = false;

    console.log(this.formGroup);

    if (this.formGroup.invalid) {

      if( this.data.isPC && this.data.userEntity.length == 0 ){
        this.userEntityRequired = true;
      }

      await this.markFormGroupTouched(this.formGroup);
      this.messageService.errorPopup('กรุณากรอกข้อมูลให้สมบูรณ์');
      return;
    }

    if( this.data.isPC && this.data.userEntity.length == 0 ){
      this.messageService.errorPopup('กรุณาเพิ่มข้อมูล User Entity');
      this.userEntityRequired = true;
      return;
    }

    if( !this.data.isPC ){
      this.data.userEntity = [];
      this.branchGroupLists.forEach(group => {
        group.branchList.forEach(branch => {
          if( branch.isSelected ){

            let userrole = {
              key: this.data.userRoleId
            }

            let userEntity = {
              userRole: userrole,
              branch: branch
            }
        
            this.data.userEntity.push(userEntity);
          }
        });
      });

      console.log(this.data.userEntity);

      if( this.data.userEntity.length == 0 ){
        this.messageService.errorPopup('กรุณาเพิ่มข้อมูลห้างสาขา');
        this.userEntityRequired = true;
        return;
      }
    }
    
    this.data.createUser = this.user.username;

    console.log(this.data);

    let res: any = await this._userService.create(this.data);
    console.log(res);

    await this.messageService.successPopup(res.message);

    this.router.navigateByUrl('/app/user');

  }

}
