import { ViewSpecDrawingComponent } from './../view-spec-drawing/view-spec-drawing.component';
import { SpecDrawingComponent } from './../../spec-drawing/spec-drawing/spec-drawing.component';
import { Observable } from 'rxjs';
import { DatePipe } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA, PageEvent } from '@angular/material';
import { Router } from '@angular/router';
import { AppSetting } from '../../_constants/app-setting';
import { OrderSummaryParamView, OrderSummaryProdDetailView, OrderSummaryProductDetailView, OrderSummarySearchView, SpecDrawingView } from '../../_model/job-operation';
import { AuthenticationService } from '../../_service/authentication.service';
import { JobOrderSummaryService } from '../../_service/job-order-summary.service';
import { JobOrderDetailComponent } from '../job-order-detail/job-order-detail.component';


declare var require: any
const FileSaver = require('file-saver');

@Component({
  selector: 'app-job-order-summary',
  templateUrl: './job-order-summary.component.html',
  styleUrls: ['./job-order-summary.component.scss']
})
export class JobOrderSummaryComponent implements OnInit {

  public model: OrderSummarySearchView = new OrderSummarySearchView(); 
  public dataOrderSummary: OrderSummaryProdDetailView<OrderSummaryProductDetailView> = new OrderSummaryProdDetailView<OrderSummaryProductDetailView>();
  public specDrawingData: SpecDrawingView = new SpecDrawingView();

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

  ngOnInit() {

    this.user = this._authSvc.getLoginUser(); 

     console.log("paramData.build_type : " + this.paramData.build_type);
     console.log("paramData.pdjit_grp : " + this.paramData.pdjit_grp);
     console.log("paramData.wc_code : " + this.paramData.wc_code);
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


    this.dataOrderSummary =  await this._jobOrderSummarySvc.searchOrderSummary(this.model);
    //console.log(this.dataOrderSummary.productDetail);
    console.log(this.dataOrderSummary);

    /*
    this.dataPending =  await this._jobOperationMacSvc.searchJobOperationPending(this.model);
    console.log(this.dataPending);
    this.dataForward =  await this._jobOperationMacSvc.searchJobOperationForward(this.model);
    console.log(this.dataForward); */
     
  }


  openOrderInfoDialog(p_entityCode: string, p_porNo: string, _isEdit: boolean = false, _index: number = -1) {
    const dialogRef = this._dialog.open(JobOrderDetailComponent, {
      maxWidth: '100vw',
      maxHeight: '100vh', 
      height: '100%',
      width: '100%',
      data: {
        entity_code: p_entityCode,
        por_no: p_porNo,
        isEdit: _isEdit,
       // editItem: _editItem,
        hideSerialNo: true,
        isSaleBed: false
      }
    });
  
    dialogRef.afterClosed().subscribe(result => { 
      if (result) {
        
      }
    });
  }

  openSpecDrawingDialog(p_barCode: string, _isEdit: boolean = false, _index: number = -1) {
    const dialogRef = this._dialog.open(ViewSpecDrawingComponent, {
      maxWidth: '100vw',
      maxHeight: '100vh', 
      height: '100%',
      width: '100%',
      data: {
        bar_code: p_barCode,
        isEdit: _isEdit,
       // editItem: _editItem,
        hideSerialNo: true,
        isSaleBed: false
      }
    });
  
    dialogRef.afterClosed().subscribe(result => { 
      if (result) {
        
      }
    });
  }

  getSpecDrawingURL(pBarcode: string) : string {
      var  vURL : string = "";
      //this.getSpecDrawing(pBarcode);

      //vURL = this.specDrawingData.file_name;

      vURL = "\\\\192.168.8.20\\DataCenter\\DataScan\\ITD\\Test.pdf";
    
      return vURL; 
    }  
  
  /*
  openSpecDrawing(PBarcode: string) { 
    //fileURL = "D:\data\Test.pdf";  //"\\\\192.168.8.20\\DataCenter\\DataScan\\ITD\\Test.pdf";
    //fileURL = "file://192.168.8.20/DataCenter/DataScan/ITD/"; // @"\"
    //fileURL = "http://file://///192.168.8.20/DataCenter/DataScan/ITD/Test.pdf";
    //fileURL = "http:////file://192.168.8.20/DataCenter/DataScan/ITD/Test.pdf";
    //fileURL = "http:\\\\file:\\\\192.168.8.20\\DataCenter\\DataScan\\ITD\\Test.pdf";
    //fileURL   = "http://192.168.9.5:8887/Test.pdf";
    //fileURL = "file://///192.168.8.20/DataCenter/DataScan/ITD/Test.pdf";
    var fileURL = "file://///192.168.8.20/DataCenter/DataScan/ITD/Test.pdf"

   // file:\\\\192.168.8.20\\DataCenter\\DataScan\\ITD\\Test.pdf
   //\\\\192.168.8.20\\DataCenter\\DataScan\\ITD\\Test.pdf
    //window.open(fileURL);
    window.location.href = fileURL;
  
  }

  
  downloadPdf() {
    /*const pdfUrl = './assets/Test.pdf';
    const pdfName = 'filename';
    FileSaver.saveAs(pdfUrl, pdfName); */

  /*  var blob = new Blob(["Hello, world!"], {type: "text/plain;charset=utf-8"});
    FileSaver.saveAs(blob, "hello world.txt");
    
  }
  */

 

}
