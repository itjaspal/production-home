import { BranchView } from './../../_model/branch';
import { ActivatedRoute } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { OrganizationService } from '../../_service/organization.service';

@Component({
  selector: 'app-branch-view',
  templateUrl: './branch-view.component.html',
  styles: []
})
export class BranchViewComponent implements OnInit {

  constructor(
    private _orgSvc: OrganizationService,
    private _activateRoute: ActivatedRoute
  ) { }

  public model: BranchView = new BranchView();
  public branchId: number = 0;

  async ngOnInit() {

    this.branchId = this._activateRoute.snapshot.params.branchId;
    this.model = await this._orgSvc.getBranchInfo(this.branchId);

  }

  close() {
    window.history.back();
  }

}
