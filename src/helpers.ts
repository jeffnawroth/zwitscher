import { Base64 } from "js-base64";
export function generateFileURL(
  file: string | File | null | undefined
): string | undefined {
  if (!file) return undefined;

  if (typeof file === "string") {
    const encoded = file.replace(/^data:(.*,)?/, "");
    const uInt8Array = Base64.toUint8Array(encoded);
    return URL.createObjectURL(new Blob([uInt8Array]));
  } else if (file instanceof File) {
    return URL.createObjectURL(file);
  }
}

export const toBase64 = (file: File) =>
  new Promise((resolve, reject) => {
    if (typeof file === "string") return resolve(file);
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => resolve(reader.result);
    reader.onerror = reject;
  });

export const filesToBase64 = (files: File[]) =>
  Promise.all(files.map((file) => toBase64(file)));
