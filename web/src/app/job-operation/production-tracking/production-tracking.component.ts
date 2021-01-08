import { Component, OnInit, Inject } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from "@angular/material";
import { Router, ActivatedRoute } from "@angular/router";
import { ProductionTrackingSearchView } from "../../_model/job-operation";
import { AuthenticationService } from "../../_service/authentication.service";
import { JobOperationService } from "../../_service/job-operation.service";
import { MessageService } from "../../_service/message.service";
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-production-tracking',
  templateUrl: './production-tracking.component.html',
  styleUrls: ['./production-tracking.component.scss']
})
export class ProductionTrackingComponent implements OnInit {

  constructor(    
    public dialogRef: MatDialogRef<any>,
    @Inject(MAT_DIALOG_DATA) public data: ProductionTrackingSearchView,
    private _authSvc: AuthenticationService,
    private _dialog: MatDialog,
    private _msgSvc: MessageService,
    private _router: Router,
    private _actRoute: ActivatedRoute,
    private _jobSvc: JobOperationService,

  ) { }

  public user: any;
  public searchModel: ProductionTrackingSearchView = new ProductionTrackingSearchView();
  public datas: any = {};
  public datePipe = new DatePipe('en-US');
  
  ngOnInit() {
   
    this.searchProductionTracking();
  }

  async searchProductionTracking()
  {
   
    this.searchModel.entity_code = this.data.entity_code;
    this.searchModel.req_date  = this.data.req_date;
    this.searchModel.wc_code = this.data.wc_code;
    this.searchModel.pdjit_grp = this.data.pdjit_grp;
    this.searchModel.build_type = this.data.build_type;

    console.log(this.searchModel);
    this.datas =  await this._jobSvc.searchProductionTracking(this.searchModel);
    console.log(this.datas);
  }

  close() { 
    this.dialogRef.close();
  }
}
