import { BranchGroupService } from '../../_service/branch-group.service';
import { BranchGroupView } from '../../_model/branchGroup';
import { MatDialog } from '@angular/material';
import { forkJoin } from 'rxjs';
import { DropdownlistService } from '../../_service/dropdownlist.service';
import { Dropdownlist } from '../../_model/dropdownlist';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MessageService } from '../../_service/message.service';

@Component({
    selector: 'app-branch-group-update',
    templateUrl: './branch-group-update.component.html',
    styles: []
  })
export class BranchGroupUpdateComponent implements OnInit {

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
  public id : number = undefined;
   
  async ngOnInit() {
   
    this.id = this._activateRoute.snapshot.params.id;
    
    this.buildForm();

    if (this.id != undefined) {
      this.model = await this._branchGroupSvc.getInfo(this.id);
    }
  }

  buildForm() {
    this.validationForm = this._formBuilder.group({
      branchGroupCode: [null, []],
      branchGroupName: [null, [Validators.required]],      
    });
  }

  close() {
    window.history.back();
  }

  async save() {

    await this._branchGroupSvc.update(this.model);

    await this._msgSvc.successPopup("บันทึกข้อมูลเรียบร้อย");
    this._router.navigateByUrl('/app/branch-group');

  }

}
