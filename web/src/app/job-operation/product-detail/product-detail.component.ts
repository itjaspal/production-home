import { DatePipe } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA, PageEvent } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import { ViewSpecComponent } from '../../product-defect/view-spec/view-spec.component';
import { OrderInfoHeaderView, OrderDetailParamView } from '../../_model/job-operation';
import { AuthenticationService } from '../../_service/authentication.service';
import { JobOrderDetailService } from '../../_service/job-order-detail.service';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.scss']
})
export class ProductDetailComponent implements OnInit {

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

     console.log("paramData.entity_code : " + this.paramData.entity_code);
     console.log("paramData.por_no : " + this.paramData.por_no);

     this.searchOrderSummary();
  }

  openSpec(p_bar_code : string , p_dsgn_no : string  , _index: number = -1)
  {
    console.log(p_bar_code);
    const dialogRef = this._dialog.open(ViewSpecComponent, {
      maxWidth: '100vw',
      maxHeight: '100vh',
      height: '100%',
      width: '100%',
      data: {    
        bar_code: p_bar_code,
        dsgn_no :p_dsgn_no
      }

    });

    
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
