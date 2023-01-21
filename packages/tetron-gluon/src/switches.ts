import { BrowserEngine } from "@gluon-framework/gluon";

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

const SWITCHES: Record<BrowserEngine, string[]> = {
    "chromium": [
        "-test-type", // Hide warnings about unsupported flags
        "--disable-gpu-sandbox",
        "--enable-webgl2-compute-context",
        "--lang en-US",
        "--no-sandbox",
        "--force-discrete-gpu 1",
        "--enable-high-resolution-time",
        "--enable-zero-copy",
        "--ignore-gpu-blacklist",
        "--autoplay-policy no-user-gesture-required"
    ],
    "firefox": [
        // TODO: Add Firefox switches
    ]
};

export default function getSwitches(engine: BrowserEngine): string[] {
    return SWITCHES[engine];
}
