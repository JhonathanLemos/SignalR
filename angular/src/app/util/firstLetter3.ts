export class FirstLetter {
    transform(value: string): string {
        if (!value) return '';
        return value.charAt(0);
      }
}