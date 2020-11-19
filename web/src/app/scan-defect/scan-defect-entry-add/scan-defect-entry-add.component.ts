import { DatePipe } from '@angular/common';
import { ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material';
import { Router, ActivatedRoute } from '@angular/router';
import { ScanInprocessSearchView } from '../../_model/job-inprocess-stock';
import { ScanDefectView, ScanDefectSearchView, ScanDefectScanFinView, ScanDefectScanDataView } from '../../_model/scan-defect';
import { AuthenticationService } from '../../_service/authentication.service';
import { MessageService } from '../../_service/message.service';
import { ScanDefectService } from '../../_service/scan-defect.service';
import { ScanDefectRemarkComponent } from '../scan-defect-remark/scan-defect-remark.component';
import { SubProductDefectSearchComponent } from '../sub-product-defect-search/sub-product-defect-search.component';

@Component({
  selector: 'app-scan-defect-entry-add',
  templateUrl: './scan-defect-entry-add.component.html',
  styleUrls: ['./scan-defect-entry-add.component.scss']
})
export class ScanDefectEntryAddComponent implements OnInit {

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
    private cdr: ChangeDetectorRef,

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

  


  ngOnInit() {
    this.buildForm();
    this.user = this._authSvc.getLoginUser();
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
    console.log(this.searchModel);
  }

  private buildForm() {
    this.validationForm = this._fb.group({
      sub_prod_code: [null, [Validators.required]],
      qty: [null, []]
    });
  }


  async save() {

    if(this.searchModel.qty == 0)
    {
      this._msgSvc.warningPopup("ต้องใส่จำนวน");
    }
    else
    {
      var datePipe = new DatePipe("en-US");
    this.searchModel.entity = this.data.entity;
    this.searchModel.wc_code = this.data.wc_code;
    this.searchModel.req_date = datePipe.transform(this.data.req_date, 'dd/MM/yyyy').toString();
    this.searchModel.por_no = this.data.por_no;
    this.searchModel.ref_no = this.data.ref_no;
    this.searchModel.user_id = this.user.username;
    this.searchModel.build_type = this.user.branch.entity_code;
    console.log(this.data);

    this.datas = await this._defectSvc.searchEntryAdd(this.searchModel);

    this.add(this.datas);
    this.searchModel.sub_prod_code = "";
    this.searchModel.qty = 0;
    }

    

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



  }


  openSearchProductModal() {

    const dialogRef = this._dialog.open(SubProductDefectSearchComponent, {
      maxWidth: '100vw',
      maxHeight: '100vh',
      height: '80%',
      width: '80%',
      data: {
        entity: this.data.entity,
        req_date: this.searchModel.req_date,
        wc_code: this.data.wc_code,
        por_no: this.searchModel.por_no,
        ref_no: this.searchModel.ref_no,
      }

    });

    dialogRef.afterClosed().subscribe(result => {
      if (result.length > 0) {
        this.add_prod(result);
      }
    })
  }

  add_prod(datas: any) {

    console.log(datas);
    this.searchModel.sub_prod_code = datas[0].sub_prod_code;
    this.searchModel.prod_code = datas[0].prod_code;
    this.searchModel.qty = 0;
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

  openRemarkModal(p_item_no : number) {

    const dialogRef = this._dialog.open(ScanDefectRemarkComponent, {
      maxWidth: '100vw',
      maxHeight: '100vh',
      height: '80%',
      width: '80%',
      data: {
        entity: this.data.entity,
        req_date: this.searchModel.req_date,
        wc_code: this.data.wc_code,
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
