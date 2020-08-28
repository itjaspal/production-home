import { BranchGroupService } from './../../_service/branch-group.service';
import { BranchGroupView } from './../../_model/branchGroup';
import { MatDialog } from '@angular/material';
import { forkJoin } from 'rxjs';
import { DropdownlistService } from './../../_service/dropdownlist.service';
import { Dropdownlist } from './../../_model/dropdownlist';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MessageService } from '../../_service/message.service';

@Component({
    selector: 'app-branch-group-create',
    templateUrl: './branch-group-create.component.html',
    styles: []
  })
export class BranchGroupCreateComponent implements OnInit {

  constructor(
    private _branchGroupSvc: BranchGroupService,
    private _activateRoute: ActivatedRoute,
    private _formBuilder: FormBuilder,
    private _ddlSvc: DropdownlistService,
    private _msgSvc: MessageService,
    private _router: Router
  ) { }

  public model: BranchGroupView = new BranchGroupView();
  public validationForm: FormGroup;
   
  async ngOnInit() {
     
    this.buildForm();
  }

  buildForm() {
    this.validationForm = this._formBuilder.group({
      branchGroupCode: [null, [Validators.required]],
      branchGroupName: [null, [Validators.required]],      
    });
  }

  close() {
    window.history.back();
  }

  async save() {

    await this._branchGroupSvc.create(this.model);

    await this._msgSvc.successPopup("บันทึกข้อมูลเรียบร้อย");
    this._router.navigateByUrl('/app/branch-group');

  }

}
