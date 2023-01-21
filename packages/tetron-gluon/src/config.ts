/* eslint-disable @typescript-eslint/no-explicit-any */
import { existsSync, readFileSync, writeFileSync } from "fs";
import { join } from "path";
import { IConfig } from "../../tetron-common/src/config.js";

export class Config implements IConfig {
    cfg: any;

    constructor(cfg?: any) {
        if (!cfg) {
            cfg = {
                url: "https://tetr.io/",
                windowSize: [1600, 800]
            };
        }

        this.cfg = cfg;
    }

    get<T>(key: string): T {
        return this.cfg[key] as T;
    }

    set<T>(key: string, value: T): void {
        this.cfg[key] = value;
    }

    static read(name?: string | undefined, dir?: string | undefined): Config {
        name ??= "config.json";
        dir ??= process.cwd();

        const fullPath = join(dir, name);

        return existsSync(fullPath) ? new Config(JSON.parse(readFileSync(fullPath, "utf8"))) : new Config();
    }

    static write(config: Config, name?: string | undefined, dir?: string | undefined): void {
        name ??= "config.json";
        dir ??= process.cwd();

        const fullPath = join(dir, name);
        writeFileSync(fullPath, JSON.stringify(config, null, 4));
    }
}
