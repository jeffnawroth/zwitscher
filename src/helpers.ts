export function generateFileURL(file: File | null | undefined) {
  if (!file) return;
  return URL.createObjectURL(file);
}
