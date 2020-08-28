import { FormGroup, ValidationErrors } from "@angular/forms";
import { ElementRef } from "@angular/core";

export class BaseComponent {
  public nav: any = []; 


  async markFormGroupTouched(formGroup: FormGroup) {
    (<any>Object).values(formGroup.controls).forEach(control => {
      control.markAsTouched();
      if (control.controls) {
        this.markFormGroupTouched(control);
      }
    });
  }

  async markFormGroupUnTouched(formGroup: FormGroup) {
    (<any>Object).values(formGroup.controls).forEach(control => {
      control.markAsUntouched();
      if (control.controls) {
        this.markFormGroupTouched(control);
      }
    });
  }

  getFormValidationErrors(formGroup: FormGroup) {
    Object.keys(formGroup.controls).forEach(key => {

      const controlErrors: ValidationErrors = formGroup.get(key).errors;
      if (controlErrors != null) {
        Object.keys(controlErrors).forEach(keyError => {
          console.log('Key control: ' + key + ', keyError: ' + keyError + ', err value: ', controlErrors[keyError]);
        });
      }
    });
  }

  public static isZZCode(_txt) {
    if (_txt == null || _txt == "") {
      return false;
    } else {

      if (_txt.length >= 2) {

        let prefix = _txt.substring(0, 2);

        if (prefix.toLowerCase() == "zz") {
          return true;
        } else {
          return false;
        }

      } else {
        return false;
      }

    }
  }


  public static exportToCSV(title: string, tbExports: ElementRef, fileName: string = "data") {
    console.log(tbExports)
    let tb_count = tbExports.nativeElement.children.length;

    let csvString = '"' + title + '"';

    for (let i = 0; i < tb_count; i++) {

      csvString += "\n\n";

      let rw_count = tbExports.nativeElement.children[i].rows.length;

      for (let j = 0; j < rw_count; j++) {

        for (let data of tbExports.nativeElement.children[i].rows[j].cells) {

          csvString += '"' + data.innerText + '",';
          if (data.colSpan > 1) {
            for (let span = 0; span < data.colSpan - 1; span++) {
              csvString += ',';
            }
          }
        }

        csvString = csvString.substring(0, csvString.length - 1);
        csvString = csvString + "\n";

      }
      csvString = csvString.substring(0, csvString.length - 1);

    }


    var linkElement = document.createElement('a');
    var universalBOM = "\uFEFF";

    linkElement.setAttribute('href', 'data:application/octet-stream;base64,' + window.btoa(unescape(encodeURIComponent(universalBOM + csvString))));
    // linkElement.setAttribute("download", filename);
    linkElement.setAttribute("download", fileName + ".csv");

    var clickEvent = new MouseEvent("click", {
      "view": window,
      "bubbles": true,
      "cancelable": false
    });

    linkElement.dispatchEvent(clickEvent);

  }

}
