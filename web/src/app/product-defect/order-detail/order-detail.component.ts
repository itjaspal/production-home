import { DatePipe } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA, PageEvent } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import { OrderDetailParamView, OrderInfoHeaderView } from '../../_model/job-operation';
import { AuthenticationService } from '../../_service/authentication.service';
import { JobOrderDetailService } from '../../_service/job-order-detail.service';

@Component({
  selector: 'app-order-detail',
  templateUrl: './order-detail.component.html',
  styleUrls: ['./order-detail.component.scss']
})
export class OrderDetailComponent implements OnInit {

  constructor(
    private _jobOrderDetailSvc: JobOrderDetailService,
    private _authSvc: AuthenticationService,
    private _activateRoute: ActivatedRoute,
    private _router: Router,
    private _formBuilder: FormBuilder,
    private _dialog: MatDialog,
    public dialogRef: MatDialogRef<any>,
    @Inject(MAT_DIALOG_DATA) public paramData: OrderDetailParamView
  ) { }

  public dataOrderInfo: OrderInfoHeaderView = new OrderInfoHeaderView();
  public user: any;
  public datePipe = new DatePipe('en-US');

  ngOnInit() {

    this.user = this._authSvc.getLoginUser(); 

    console.log("paramData.entity_code : " + this.paramData.entity_code);
    console.log("paramData.por_no : " + this.paramData.por_no);

     this.searchOrderSummary();
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

 close() { 
   this.dialogRef.close();
 }


}
