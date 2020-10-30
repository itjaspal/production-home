import { PrintInprocessDetailComponent } from './../../print-inprocess-tag/print-inprocess-detail/print-inprocess-detail.component';
import { ChangeDetectorRef, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { DatePipe } from '@angular/common';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material';
import { Router, ActivatedRoute } from '@angular/router';
import { ProductSearchComponent } from '../../scan-inprocess/product-search/product-search.component';
import { JobInProcessScanView } from '../../_model/job-inprocess';
import { ScanSendView, ScanSendSearchView, ScanSendFinView, ScanSendFinDataView, SetNoView, PrintSetNoView } from '../../_model/scan-send';
import { AuthenticationService } from '../../_service/authentication.service';
import { MessageService } from '../../_service/message.service';
import { ScanSendService } from '../../_service/scan-send.service';
import { DropdownlistService } from '../../_service/dropdownlist.service';
import * as moment from 'moment';
import { ScanSendSetnoSearchComponent } from '../scan-send-setno-search/scan-send-setno-search.component';


@Component({
  selector: 'app-scan-send',
  templateUrl: './scan-send.component.html',
  styleUrls: ['./scan-send.component.scss']
})
export class ScanSendComponent implements OnInit {

  @ViewChild('req_date') req_date: ElementRef;
  
  constructor(
    private _fb: FormBuilder,
    private _authSvc: AuthenticationService,
    private _dialog: MatDialog,
    private _msgSvc: MessageService,
    private _router: Router,
    private _actRoute: ActivatedRoute,
    private _scanSendSvc: ScanSendService,
    private cdr: ChangeDetectorRef,
    private _dll: DropdownlistService,
  ) { }

  public validationForm: FormGroup;
  public user: any;
  public model: ScanSendView = new ScanSendView();
  public searchModel: ScanSendSearchView = new ScanSendSearchView();

  public data: any = {};
  public model_scan: ScanSendFinView = new ScanSendFinView();
  public model_setno : PrintSetNoView = new PrintSetNoView();
  public datas: any = {};
  public datas_print: any = {};
  public wclist: any; 

  public data_qty : any;
  
  async ngOnInit() {
    this.buildForm();
    this.user = this._authSvc.getLoginUser();
    this.wclist = await this._dll.getDdlWCSend(this.user.username);
    if(this.wclist.length==1)
    {
      this.searchModel.wc_code = this.wclist[0].key;
    }
    //this.searchModel.req_date = new Date()
    console.log(this.data);
    
  }

  private buildForm() {
    this.validationForm = this._fb.group({
      pcs_barcode: [null, [Validators.required]],
      wc_code: [null, [Validators.required]],
      req_date: [null, [Validators.required]],
      set_no:[null, []]
    });
  }

  async onQrEntered(_pcs_barcode: string) {

    if (_pcs_barcode == null || _pcs_barcode == "") {
      return;
    }

    var datePipe = new DatePipe("en-US");
    this.searchModel.req_date = datePipe.transform(this.searchModel.req_date, 'dd/MM/yyyy').toString();
    this.searchModel.user_id = this.user.username;
    console.log(this.searchModel);
    
    this.datas = await this._scanSendSvc.scansendadd(this.searchModel);

    console.log(this.datas); 

    this.add(this.datas);
    this.searchModel.pcs_barcode = "";
    this.model.show_qty = this.datas.scan_qty + " / " + this.datas.set_qty;
    this.searchModel.req_date = this.datas.req_date; 
    this.searchModel.build_type = this.user.branch.entity_code;
    if(this.datas.set_qty == this.datas.scan_qty)
    {
    
      this.datas_print = await this._scanSendSvc.PrintSticker(this.datas);
      this.model.show_qty = "";
      
    }
    this.model.set_no = ""; 
    
  }

  async save() {

    var datePipe = new DatePipe("en-US");
    this.searchModel.req_date = datePipe.transform(this.searchModel.req_date, 'dd/MM/yyyy').toString();
    this.searchModel.user_id = this.user.username;
    console.log(this.searchModel);
    
    this.datas = await this._scanSendSvc.scansendadd(this.searchModel);

    console.log(this.datas);

    this.add(this.datas);
    this.searchModel.pcs_barcode = "";
    this.model.show_qty = this.datas.scan_qty + " / " + this.datas.set_qty;
    this.searchModel.req_date = this.datas.req_date; 
    this.searchModel.build_type = this.user.branch.entity_code;
    if(this.datas.set_qty == this.datas.scan_qty)
    {
    
      this.datas_print = await this._scanSendSvc.PrintSticker(this.datas);
      this.model.show_qty = "";
      
    }
    this.model.set_no = "";


  }

  add(datas: any) {

    let newProd: ScanSendFinDataView = new ScanSendFinDataView();
    newProd.prod_code = datas.prod_code;
    newProd.prod_name = datas.prod_name;
    newProd.pcs_barcode = datas.pcs_barcode;
    newProd.job_no = datas.job_no;
    newProd.wc_code = datas.wc_code;
    newProd.req_date = datas.req_date;
    newProd.entity = datas.entity;

    this.model_scan.datas.push(newProd);  
    console.log(this.model_scan.datas);

  }

  async print(){
    var datePipe = new DatePipe("en-US");
    this.datas.set_qty = this.datas.scan_qty;
    if(this.model.set_no == "")
    {
      this.datas_print = await this._scanSendSvc.PrintSticker(this.datas);
    }
    else
    {
      this.model_setno.set_no = this.model.set_no;
      this.model_setno.req_date = datePipe.transform(this.searchModel.req_date, 'dd/MM/yyyy').toString();
      this.model_setno.wc_code = this.searchModel.wc_code;
      this.model_setno.user_id = this.user.username;
      this.model_setno.set_qty = this.model.set_qty;
      this.model_setno.scan_qty = this.model.scan_qty;
      console.log(this.model_setno);
      
      this.datas_print = await this._scanSendSvc.RePrintSticker(this.model_setno);
      
      this.model.set_no = "";
    }
    
    this.model.show_qty = "";
  }

  openSearchSetNoModal(p_entity : string ,p_req_date: string, p_wc_code: string, _index: number = -1)
  {
    const dialogRef = this._dialog.open(ScanSendSetnoSearchComponent, {
      maxWidth: '100vw',
      maxHeight: '100vh',
      height: '80%',
      width: '80%',
      data: {
        tran_date: p_req_date,
        wc_code:p_wc_code,
        entity:p_entity
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
    this.model.set_no = datas[0].set_no;
    this.model.show_qty = datas[0].scan_qty + " / " + datas[0].set_qty;
    this.model_scan = datas[0];
    this.model.set_qty = datas[0].set_qty;
    this.model.scan_qty = datas[0].scan_qty;
    // this.searchModel.qty = 1;
  }  


  async delete(_index,scan) {
    console.log(scan); 

    this._msgSvc.confirmPopup("ยืนยันยกเลิกบันทึกส่งมอบ", async result => {
      if (result) {
        let res: any = await this._scanSendSvc.delete(scan);

        this._msgSvc.successPopup(res.message);

        this.data_qty = await this._scanSendSvc.getscanqty(scan);

        console.log(this.data_qty);
        this.model.show_qty = this.data_qty.scan_qty + " /" + this.data_qty.set_qty;
        this.model_scan.datas.splice(_index, 1);

      }
    })

  }


  close() {
    this._router.navigateByUrl('/app/home');
  }


}
