import { Component, ElementRef, Inject, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import { JobInProcessStockScanFinView, JobInProcessStockScanView, JobInProcessStockSearchView, JobInProcessStockView, ScanInprocessSearchView } from '../../_model/job-inprocess-stock';
import { AuthenticationService } from '../../_service/authentication.service';
import { MessageService } from '../../_service/message.service';
import { ScanDefectService } from '../../_service/scan-defect.service';
import { DatePipe } from '@angular/common';
import { ScanDefectView, ScanDefectSearchView, ScanDefectScanFinView, ScanDefectScanDataView } from '../../_model/scan-defect';
import { ScanDefectRemarkComponent } from '../scan-defect-remark/scan-defect-remark.component';

@Component({
  selector: 'app-scan-defect-scan-add',
  templateUrl: './scan-defect-scan-add.component.html',
  styleUrls: ['./scan-defect-scan-add.component.scss']
})
export class ScanDefectScanAddComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<any>,
    @Inject(MAT_DIALOG_DATA) public data: ScanInprocessSearchView,
    private _fb: FormBuilder,
    private _authSvc: AuthenticationService,
    private _dialog: MatDialog,
    private _msgSvc: MessageService,
    private _router: Router,
    private _actRoute: ActivatedRoute,
    private _defectSvc: ScanDefectService,
    //private snackBar: MatSnackBar
  ) { }

  public validationForm: FormGroup;
  public user: any;
  public model: ScanDefectView = new ScanDefectView();
  public searchModel: ScanDefectSearchView = new ScanDefectSearchView();

  // public data: any = {};
  public model_scan: ScanDefectScanFinView = new ScanDefectScanFinView();

  public datas: any = {};
  public count = 0;
  public current_date = "";
  public current_time = "";

  // public result: any = {};

  @ViewChild('qr') qrElement: ElementRef;

  ngAfterViewInit() {
    //this.qrElement.nativeElement.focus();
  }

  ngOnInit() {
    this.buildForm();
    this.user = this._authSvc.getLoginUser();
    var datePipe = new DatePipe("en-US");

    this.current_date = datePipe.transform(new Date(), 'dd/MM/yyyy').toString();
    this.current_time = datePipe.transform(new Date(), 'HH:mm').toString();

    this.searchModel.entity = this.data.entity;
    this.searchModel.wc_code = this.data.wc_code;
    this.searchModel.req_date = datePipe.transform(this.data.req_date, 'dd/MM/yyyy').toString();
    this.searchModel.por_no = this.data.por_no;
    this.searchModel.ref_no = this.data.ref_no;
    this.searchModel.user_id = this.user.username;
    this.searchModel.build_type = this.user.branch.entity_code;
    console.log(this.data);
  }

  private buildForm() {
    this.validationForm = this._fb.group({
      qr: [null, []],
      qty: [null, []],
    });
  }

  async onQrEntered(_qr: string) {

    if (_qr == null || _qr == "") {
      return;
    }

    var datePipe = new DatePipe("en-US"); 

    //  this.searchModel.entity = this.data.entity;
    //  this.searchModel.wc_code = this._actRoute.snapshot.params.wc_code;
    this.searchModel.scan_data = _qr;
    //  this.searchModel.req_date  = datePipe.transform(this.data.req_date, 'dd/MM/yyyy').toString();

    //  this.searchModel.user_id = this.user.username;
    //  this.searchModel.build_type = this.user.branch.entity_code;



    this.datas = await this._defectSvc.searchScanAdd(this.searchModel);
    console.log(this.datas);
    this.qrElement.nativeElement.focus();


    this.add(this.datas);

  }

  add(datas: any) {

    let newProd: ScanDefectScanDataView = new ScanDefectScanDataView();
    newProd.prod_code = datas.prod_code;
    newProd.sub_prod_code = datas.sub_prod_code;
    newProd.sub_prod_name = datas.sub_prod_name;
    newProd.por_no = datas.por_no;
    newProd.ref_no = datas.ref_no;
    newProd.entity = datas.entity;
    newProd.qty = datas.qty;
    newProd.wc_code = datas.wc_code;
    newProd.item_no = datas.item_no;

    this.model_scan.datas.push(newProd);
    console.log(this.model_scan);
    


  }

  cancel(_index, scan) {
    console.log(scan);

    this._msgSvc.confirmPopup("ยืนยันยกเลิกบันทึกรับมอบ", async result => {
      if (result) {
        let res: any = await this._defectSvc.Cancel(scan);

        this._msgSvc.successPopup(res.message);
        this.model_scan.datas.splice(_index, 1);

      }
    })
  }


  openRemarkModal(p_item_no : number ) {

    const dialogRef = this._dialog.open(ScanDefectRemarkComponent, {
      maxWidth: '100vw',
      maxHeight: '100vh',
      height: '80%',
      width: '80%',
      data: {
        entity: this.data.entity,
        req_date: this.searchModel.req_date,
        por_no: this.searchModel.por_no,
        ref_no: this.searchModel.ref_no,
        item_no : p_item_no
      }

    });

    // dialogRef.afterClosed().subscribe(result => {
    //   if (result.length > 0) {
    //     this.add_prod(result);
    //   }
    // })
  }

  close() {
    this.dialogRef.close();
  }


}
