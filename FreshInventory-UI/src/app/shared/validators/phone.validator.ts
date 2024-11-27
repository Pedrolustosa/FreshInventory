import { AbstractControl, ValidationErrors } from '@angular/forms';

export function phoneNumberValidator(control: AbstractControl): ValidationErrors | null {
  const value = control.value;
  
  if (!value) {
    return null;
  }

  // Remove all non-numeric characters for validation
  const numericValue = value.replace(/\D/g, '');
  
  // Check if the phone number has a valid length (between 10 and 11 digits)
  const isValidLength = numericValue.length >= 10 && numericValue.length <= 11;
  
  // Check if the phone number starts with valid Brazilian area codes
  const hasValidAreaCode = /^([1-9][1-9])/.test(numericValue);
  
  if (!isValidLength || !hasValidAreaCode) {
    return { invalidPhone: true };
  }
  
  return null;
}
