# main Branch Protection Kurulumu

main branch'i korumak için GitHub'da şu adımları izle:

**→ [Branch protection ayarlarına git](https://github.com/ibrahimKaya66/arac_ilani/settings/branches)**

1. **Add branch protection rule** veya **Add rule** butonuna tıkla
2. Branch name pattern: `main`
3. İşaretle:
   - ✅ **Do not allow bypassing the above settings** (admin dahil)
   - ✅ **Do not allow force pushes**
   - ✅ **Do not allow deletions**
4. **Create** veya **Save changes**

İstersen ek olarak:
- **Require a pull request before merging** (PR zorunlu)
- **Require status checks to pass** (CI geçmeli)
