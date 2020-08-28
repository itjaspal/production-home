import { MessageService } from './../../_service/message.service';
import { MatDialog } from '@angular/material';
import { forkJoin } from 'rxjs';
import { DropdownlistService } from './../../_service/dropdownlist.service';
import { Dropdownlist } from './../../_model/dropdownlist';
import { BranchView } from './../../_model/branch';
import { Component, OnInit } from '@angular/core';
import { OrganizationService } from '../../_service/organization.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-branch-save',
  templateUrl: './branch-save.component.html',
  styles: []
})
export class BranchSaveComponent implements OnInit {

  constructor(
    private _orgSvc: OrganizationService,
    private _activateRoute: ActivatedRoute,
    private _formBuilder: FormBuilder,
    private _ddlSvc: DropdownlistService,
    private _msgSvc: MessageService,
    private _router: Router
  ) { }


  public branchId: number = 0;
  public model: BranchView = new BranchView();
  public validationForm: FormGroup;
  public ddlStatus: Dropdownlist[] = [];
  public ddlBranchGroup: Dropdownlist[] = [];

  async ngOnInit() {

    this.branchId = this._activateRoute.snapshot.params.branchId;
    this.model.branchGroupId = +this._activateRoute.snapshot.params.branchGroupId;

    forkJoin([
      this._ddlSvc.getDdlBranchGroup(),
      this._ddlSvc.getDdlBranchStatus()
    ]).subscribe(result => {
      this.ddlBranchGroup = result[0];
      this.ddlStatus = result[1];
    });

    this.buildForm();

    if (this.branchId != undefined) {
      this.model = await this._orgSvc.getBranchInfo(this.branchId);
    }
  }

  buildForm() {
    this.validationForm = this._formBuilder.group({
      branchCode: [null, [Validators.required]],
      branchNameThai: [null, [Validators.required]],
      branchNameEng: [null, [Validators.required]],
      //entityCode: [null, [Validators.required]],
      entityCode: [{value: []}, []],
      //branchGroupId: [{ value: '', disabled: true }, []],
      branchGroupId: [{ value: ''}, [Validators.required]],
      // email: [null, [Validators.required, Validators.pattern('^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$')]],
      email: [null, []],
      status: [null, []],
      //docRunningPrefix: [null, [Validators.required, Validators.minLength(1), Validators.maxLength(3)]],
      docRunningPrefix: [null, []],
    });

    if (this.branchId > 0) {
      this.validationForm.controls['branchCode'].clearValidators();
      this.validationForm.controls['branchCode'].updateValueAndValidity();
      //this.validationForm.controls['entityCode'].clearValidators();
      this.validationForm.controls['entityCode'].updateValueAndValidity();
    }
  }

  close() {
    window.history.back();
  }

  async save() {

    let result = await this._orgSvc.saveBranch(this.model);

    await this._msgSvc.successPopup("บันทึกข้อมูลเรียบร้อย");
    this._router.navigateByUrl('/app/branch');

  }

}
