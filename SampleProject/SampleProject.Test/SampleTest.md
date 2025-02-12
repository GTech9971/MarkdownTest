
## テスト名

ユーザー認証成功テスト

## テスト概要

有効なユーザー名とパスワードを使用した場合、認証が成功することを確認する。

## 入力値

- ユーザー名: test_user
- パスワード: password123

## 期待値

- 認証APIがステータスコード200を返却する。
- レスポンスボディにaccessTokenが含まれる。

## 前提条件

- データベースにユーザー情報が登録されていること。
- 認証APIが稼働していること。

## 実行手順

- 認証APIのエンドポイント/api/auth/loginに対してPOSTリクエストを送信する。
- 入力値をリクエストボディに含める。
- レスポンスを確認する。

## 実行環境

- OS: Windows 11
- APIホスト: <https://api.example.com>
- ツール: Postman v10.0

## コードリンク

- 実装: MarkdownTest.Cli.Parser
- テスト: MarkdownTest.Cli.Test.Parser

## テストID

---
