import { DatePipe } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material';
import { Router, ActivatedRoute } from '@angular/router';
import { ProductGroupDetailSearchView } from '../../_model/job-operation-stock';
import { AuthenticationService } from '../../_service/authentication.service';
import { JobOperationStockService } from '../../_service/job-operation-stock.service';
import { MessageService } from '../../_service/message.service';

@Component({
  selector: 'app-production-tracking-group',
  templateUrl: './production-tracking-group.component.html',
  styleUrls: ['./production-tracking-group.component.scss']
})
export class ProductionTrackingGroupComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<any>,
    @Inject(MAT_DIALOG_DATA) public data: ProductGroupDetailSearchView,
    private _authSvc: AuthenticationService,
    private _dialog: MatDialog,
    private _msgSvc: MessageService,
    private _router: Router,
    private _actRoute: ActivatedRoute,
    private _jobSvc: JobOperationStockService,
  ) { }

  public user: any;
  public searchModel: ProductGroupDetailSearchView = new ProductGroupDetailSearchView();
  public datas: any = {};
  public datas_det: any = {};
  // public datePipe = new DatePipe('en-US');

  ngOnInit() {
    this.user = this._authSvc.getLoginUser();
    this.searchProductionTracking();
  }

  async searchProductionTracking()
  {
    var datePipe = new DatePipe("en-US");

    //console.log(this.data);
    this.searchModel.entity_code = this.data.entity_code;
    //this.searchModel.req_date  =  datePipe.transform(this.data.req_date, 'dd/MM/yyyy').toString(); 
    this.searchModel.req_date  =  this.data.req_date; 
    // this.searchModel.wc_code = this.data.wc_code;
    this.searchModel.por_no = this.data.por_no;
    this.searchModel.ref_no = this.data.ref_no;
    this.searchModel.build_type = this.user.branch.entity_code;
    this.searchModel.user_id = this.data.user_id;
    this.searchModel.group = this.data.group;
    this.searchModel.wc_code = "";

    console.log(this.searchModel);
    if(this.searchModel.group == "OTHER")
    {
      this.datas_det =  await this._jobSvc.searchProductionTrackingGroupOthDetailStock(this.searchModel);
    }
    else
    {
      this.datas_det =  await this._jobSvc.searchProductionTrackingGroupDetailStock(this.searchModel);
    }
    
    console.log(this.datas_det);
  }

  close() { 
    this.dialogRef.close();
  }

}
