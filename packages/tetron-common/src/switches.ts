/* Switches used here are designed to mirror the ones used by the official app.
 * DEFAULT (Chromium flags):
 *   --disable-gpu-sandbox
 *   --enable-webgl2-compute-context
 *   --lang en-US
 *   --no-sandbox
 *   --force-discrete-gpu 1
 *   --enable-high-resolution-time
 *   --enable-zero-copy
 *   --ignore-gpu-blacklist
 *   --autoplay-policy no-user-gesture-required
 */

export const CHROMIUM_SWITCHES: string[] = [
    "--disable-gpu-sandbox",
    "--enable-webgl2-compute-context",
    "--lang en-US",
    "--no-sandbox",
    "--force-discrete-gpu 1",
    "--enable-high-resolution-time",
    "--enable-zero-copy",
    "--ignore-gpu-blacklist",
    "--autoplay-policy no-user-gesture-required"
];
