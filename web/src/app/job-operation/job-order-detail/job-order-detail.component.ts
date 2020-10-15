import { JobOrderDetailService } from './../../_service/job-order-detail.service';
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA, PageEvent } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import { OrderDetailParamView, OrderInfoHeaderView } from '../../_model/job-operation';
import { AuthenticationService } from '../../_service/authentication.service';
import { JobOperationService } from '../../_service/job-operation.service';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-job-order-detail',
  templateUrl: './job-order-detail.component.html',
  styleUrls: ['./job-order-detail.component.scss']
})
export class JobOrderDetailComponent implements OnInit {

  public dataOrderInfo: OrderInfoHeaderView = new OrderInfoHeaderView();
  public user: any;
  public datePipe = new DatePipe('en-US');

  constructor(
    private _jobOrderDetailSvc: JobOrderDetailService,
    private _authSvc: AuthenticationService,
    private _activateRoute: ActivatedRoute,
    private _router: Router,
    private _formBuilder: FormBuilder,
    private _dialog: MatDialog,
    public dialogRef: MatDialogRef<any>,
    @Inject(MAT_DIALOG_DATA) public paramData: OrderDetailParamView
    
   // @Inject(MAT_DIALOG_DATA) public paramData: ViewSpecDrawingParamView
  ) { }

  ngOnInit() {

     this.user = this._authSvc.getLoginUser(); 

     console.log("paramData.entity_code : " + this.paramData.entity_code);
     console.log("paramData.por_no : " + this.paramData.por_no);

     this.searchOrderSummary();
  }

  close() { 
    this.dialogRef.close();
  }

  async searchOrderSummary(event: PageEvent = null) {   
   
    this.dataOrderInfo.orderSpecial  = [];
    this.dataOrderInfo.orderDetail = [];
    this.dataOrderInfo.remark  = [];
    
    console.log("this.paramData.entity_code : " + this.paramData.entity_code);
    console.log("this.paramData.por_no : " + this.paramData.por_no);

    this.dataOrderInfo =  await this._jobOrderDetailSvc.getOrderInfoDetail(this.paramData.entity_code,this.paramData.por_no);
    //console.log(this.dataOrderSummary.productDetail);
    console.log(this.dataOrderInfo);
     
  }

}
