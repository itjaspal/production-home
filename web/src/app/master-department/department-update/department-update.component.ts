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
    selector: 'app-department-update',
    templateUrl: './department-update.component.html',
    styles: []
  })
export class DepartmentUpdateComponent implements OnInit {

  constructor(
    private _departmentSvc: DepartmentService,
    private _activateRoute: ActivatedRoute,
    private _formBuilder: FormBuilder,
    private _ddlSvc: DropdownlistService,
    private _msgSvc: MessageService,
    private _router: Router
  ) { }

  public ddlStatus: any;

  public model: DepartmentView = new DepartmentView();
  public validationForm: FormGroup;
  public departmentId : number = undefined;
   
  async ngOnInit() {
   
    this.departmentId = this._activateRoute.snapshot.params.departmentId;
    
    this.buildForm();

    this.ddlStatus = await this._ddlSvc.getDdlBranchStatus()

    if (this.departmentId != undefined) {
      this.model = await this._departmentSvc.getInfo(this.departmentId);
    }
  }

  buildForm() {
    this.validationForm = this._formBuilder.group({
      departmentCode: [null, []],
      departmentName: [null, [Validators.required]],      
      status: [null, [Validators.required]],
    });
  }

  close() {
    window.history.back();
  }

  async save() {

    await this._departmentSvc.update(this.model);

    await this._msgSvc.successPopup("บันทึกข้อมูลเรียบร้อย");
    this._router.navigateByUrl('/app/department');

  }

}
