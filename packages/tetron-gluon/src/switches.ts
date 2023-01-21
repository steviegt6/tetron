import { BrowserEngine } from "@gluon-framework/gluon";
import { CHROMIUM_SWITCHES } from "../../tetron-common/src/switches.js";

const SWITCHES: Record<BrowserEngine, string[]> = {
    "chromium": [
        "-test-type", // Hide warnings about unsupported flags
        ...CHROMIUM_SWITCHES
    ],
    "firefox": [
        // TODO: Add Firefox switches
    ]
};

export default function getSwitches(engine: BrowserEngine): string[] {
    return SWITCHES[engine];
}
