import { DatePipe } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA, PageEvent } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import { JobOrderDetailComponent } from '../../job-operation/job-order-detail/job-order-detail.component';
import { AppSetting } from '../../_constants/app-setting';
import { OrderSummaryParamView, OrderSummaryProdDetailView, OrderSummaryProductDetailView, OrderSummarySearchView } from '../../_model/job-operation';
import { AuthenticationService } from '../../_service/authentication.service';
import { JobOrderSummaryService } from '../../_service/job-order-summary.service';
import { PrintInprocessTagComponent } from '../print-inprocess-tag/print-inprocess-tag.component';

@Component({
  selector: 'app-print-inprocess-detail',
  templateUrl: './print-inprocess-detail.component.html',
  styleUrls: ['./print-inprocess-detail.component.scss']
})
export class PrintInprocessDetailComponent implements OnInit {

  public model: OrderSummarySearchView = new OrderSummarySearchView(); 
  public dataOrderSummary: OrderSummaryProdDetailView<OrderSummaryProductDetailView> = new OrderSummaryProdDetailView<OrderSummaryProductDetailView>();

  public user: any;
  public entityCode: any = AppSetting.entity;
  public datePipe = new DatePipe('en-US');

  constructor(
    private _jobOrderSummarySvc: JobOrderSummaryService,
    private _authSvc: AuthenticationService,
    private _formBuilder: FormBuilder,
    private _router: Router,
    public dialogRef: MatDialogRef<any>,
    private _dialog: MatDialog,
    @Inject(MAT_DIALOG_DATA) public paramData: OrderSummaryParamView
  ) {
  } 

  //dialogRef : any;

  ngOnInit() {

    this.user = this._authSvc.getLoginUser(); 

     console.log("paramData.build_type : " + this.paramData.build_type);
     console.log("paramData.pdjit_grp : " + this.paramData.pdjit_grp);
     console.log("paramData.pdjit_grp_desc : " + this.paramData.pdjit_grp_desc);
     console.log("paramData.req_date : " + this.paramData.req_date);

     this.searchOrderSummary();

  }


  close() { 
    //window.history.back();
    this.dialogRef.close();
  }

  async searchOrderSummary(event: PageEvent = null) {   

    if (event != null) {
      this.model.pageIndex = event.pageIndex;
      this.model.itemPerPage = event.pageSize;
    }
   
    this.dataOrderSummary.productDetail  = [];
    
    this.model.build_type = this.paramData.build_type;
    this.model.pdjit_grp  = this.paramData.pdjit_grp;
    this.model.req_date   = this.paramData.req_date;
    this.model.wc_code   = this.paramData.wc_code;

    console.log(this.model.req_date);

    this.dataOrderSummary =  await this._jobOrderSummarySvc.searchOrderSummary(this.model);
    //console.log(this.dataOrderSummary.productDetail);
    console.log(this.dataOrderSummary);

    /*
    this.dataPending =  await this._jobOperationMacSvc.searchJobOperationPending(this.model);
    console.log(this.dataPending);
    this.dataForward =  await this._jobOperationMacSvc.searchJobOperationForward(this.model);
    console.log(this.dataForward); */
     
  }

  openPrintTagDialog(p_entity : string ,p_req_date: string, p_bar_code: string,  p_pdjit_grp: string, _index: number = -1) {
    const dialogRef = this._dialog.open(PrintInprocessTagComponent, {
      maxWidth: '100vw',
      maxHeight: '100vh', 
      height: '100%',
      width: '100%',
      data: {
        req_date: p_req_date,
        bar_code: p_bar_code,
        entity : p_entity,
        pdjit_grp : p_pdjit_grp
      }
    }); 
  
    dialogRef.afterClosed().subscribe(result => { 
      if (result) {
        
      }
    });
  }

  openOrderInfoDialog(p_entityCode: string, p_porNo: string, _index: number = -1) {
    const dialogRef = this._dialog.open(JobOrderDetailComponent, {
      maxWidth: '100vw',
      maxHeight: '100vh', 
      height: '100%',
      width: '100%',
      data: {
        entity_code: p_entityCode,
        por_no: p_porNo,
        //isEdit: _isEdit,
       // editItem: _editItem,
        // hideSerialNo: true,
        // isSaleBed: false
      }
    });
  
    dialogRef.afterClosed().subscribe(result => { 
      if (result) {
        
      }
    });
  }


}
