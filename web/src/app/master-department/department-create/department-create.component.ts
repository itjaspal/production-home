import { MessageService } from './../../_service/message.service';
import { DepartmentService } from './../../_service/department.service';
import { DepartmentView } from './../../_model/department';
import { MatDialog } from '@angular/material';
import { forkJoin } from 'rxjs';
import { DropdownlistService } from './../../_service/dropdownlist.service';
import { Dropdownlist } from './../../_model/dropdownlist';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
    selector: 'app-department-create',
    templateUrl: './department-create.component.html',
    styles: []
  })
export class DepartmentCreateComponent implements OnInit {

  constructor(
    private _departmentSvc: DepartmentService,
    private _activateRoute: ActivatedRoute,
    private _formBuilder: FormBuilder,
    private _ddlSvc: DropdownlistService,
    private _msgSvc: MessageService,
    private _router: Router
  ) { }

  

  public model: DepartmentView = new DepartmentView();
  public validationForm: FormGroup;

  public ddlStatus: any;
   
  async ngOnInit() {
   

    this.buildForm();

    this.ddlStatus = await this._ddlSvc.getDdlBranchStatus()

    //this.model.pcSaleId = this._activateRoute.snapshot.params.pcSaleId;
        
    // if (this.branchId != undefined) {
    //   this.model = await this._orgSvc.getBranchInfo(this.branchId);
    // }
  }

  buildForm() {
    this.validationForm = this._formBuilder.group({
      departmentCode: [null, [Validators.required]],
      departmentName: [null, [Validators.required]],
      status: [null, [Validators.required]],
    });
  }

  close() {
    window.history.back();
  }

  async save() {

    await this._departmentSvc.create(this.model);

    await this._msgSvc.successPopup("บันทึกข้อมูลเรียบร้อย");
    this._router.navigateByUrl('/app/department');

  }

}
