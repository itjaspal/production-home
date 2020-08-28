import { DepartmentService } from './../../_service/department.service';
import { DepartmentView } from './../../_model/department';
import { MatDialog } from '@angular/material';
import { forkJoin } from 'rxjs';
import { DropdownlistService } from './../../_service/dropdownlist.service';
import { Dropdownlist } from './../../_model/dropdownlist';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
    selector: 'app-department-view',
    templateUrl: './department-view.component.html',
    styles: []
  })
export class DepartmentViewComponent implements OnInit {

  constructor(
    private _departmentSvc: DepartmentService,
    private _activateRoute: ActivatedRoute,
    private _ddlSvc: DropdownlistService,
  ) { }

  public model: DepartmentView = new DepartmentView();
  public departmentId : number = undefined;
   
  async ngOnInit() {
   

    this.departmentId = this._activateRoute.snapshot.params.departmentId;
       
    if (this.departmentId != undefined) {
      this.model = await this._departmentSvc.getInfo(this.departmentId);
    }
  }

  close() {
    window.history.back();
  }
  
}
