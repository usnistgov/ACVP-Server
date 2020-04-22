import { Context, Recogniser } from '.';
declare class ISO_2022 implements Recogniser {
    escapeSequences: number[][];
    name(): string;
    match(det: Context): any;
}
export declare class ISO_2022_JP extends ISO_2022 {
    name(): string;
    escapeSequences: number[][];
}
export declare class ISO_2022_KR extends ISO_2022 {
    name(): string;
    escapeSequences: number[][];
}
export declare class ISO_2022_CN extends ISO_2022 {
    name(): string;
    escapeSequences: number[][];
}
export {};
