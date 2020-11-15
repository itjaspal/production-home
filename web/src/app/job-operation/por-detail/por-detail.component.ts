import { DatePipe } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA, PageEvent } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import { OrderInfoHeaderView, OrderDetailParamView } from '../../_model/job-operation';
import { AuthenticationService } from '../../_service/authentication.service';
import { JobOrderDetailService } from '../../_service/job-order-detail.service';

@Component({
  selector: 'app-por-detail',
  templateUrl: './por-detail.component.html',
  styleUrls: ['./por-detail.component.scss']
})
export class PorDetailComponent implements OnInit {

  public dataOrderInfo: OrderInfoHeaderView = new OrderInfoHeaderView();
  public user: any;
  public datePipe = new DatePipe('en-US');
  public title_name : any;
  public doc_type : any;

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
     
     if(this.paramData.isSaleBed == true)
     {
      this.title_name = "POR Detail";
      this.doc_type = "POR No.";
     }
     else
     {
      this.title_name = "MOR Detail";
      this.doc_type = "MOR No.";
     }
     

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
