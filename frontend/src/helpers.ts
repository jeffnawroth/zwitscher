import { Base64 } from "js-base64";

/**
 * Generate a object url from a file or a base64
 * @param file
 * @returns url
 */
export function generateFileURL(
  file: string | File | null | undefined,
): string | undefined {
  if (!file) return undefined;

  if (typeof file === "string") {
    const encoded = file.replace(/^data:(.*,)?/, "");
    const uInt8Array = Base64.toUint8Array(encoded);
    const buffer = new Uint8Array(uInt8Array);
    return URL.createObjectURL(new Blob([buffer]));
  } else if (file instanceof File) {
    return URL.createObjectURL(file);
  }
}

/**
 * Convert a file to base64
 * @param file
 * @returns base64 string promise
 */
export const toBase64 = (file: File | string): Promise<string> =>
  new Promise((resolve, reject) => {
    if (typeof file === "string") return resolve(file);
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => resolve(reader.result as string);
    reader.onerror = reject;
  });

/**
 * Convert files in an array to base64
 * @param files
 * @returns Array with base64 strings
 */
export const filesToBase64 = (files: File[] | string[]) =>
  Promise.all(files.map((file) => toBase64(file)));
