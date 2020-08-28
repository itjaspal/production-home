import { BranchGroupService } from './../../_service/branch-group.service';
import { BranchGroupView } from './../../_model/branchGroup';
import { MatDialog } from '@angular/material';
import { forkJoin } from 'rxjs';
import { DropdownlistService } from './../../_service/dropdownlist.service';
import { Dropdownlist } from './../../_model/dropdownlist';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
    selector: 'app-branch-group-view',
    templateUrl: './branch-group-view.component.html',
    styles: []
  })
export class BranchGroupViewComponent implements OnInit {

  constructor(
    private _branchGrouptSvc: BranchGroupService,
    private _activateRoute: ActivatedRoute,
    private _ddlSvc: DropdownlistService,
  ) { }

  public model: BranchGroupView = new BranchGroupView();
  public id : number = undefined;
   
  async ngOnInit() {
   

    this.id = this._activateRoute.snapshot.params.id;
       
    if (this.id != undefined) {
      this.model = await this._branchGrouptSvc.getInfo(this.id);
    }
  }

  close() {
    window.history.back();
  }
  
}
